using System.Globalization;
using Application;
using Domain;
using Domain.Media;
using FluentValidation;
using Infrastructure;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Utilities;
using Presentation;
using Presentation.Pictures;
using Serilog;
using WebApi;
using WebApi.Extensions;

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console(formatProvider: CultureInfo.InvariantCulture).CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
	if (context.HttpContext.Items.TryGetValue(HttpContextItemKeys.ErrorCode, out var code))
	{
		context.ProblemDetails.Extensions.Add(ProblemDetailsExtensionKeys.ErrorCode, code);
	}
});

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Environment, builder.Logging);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddValidatorsFromAssemblies([typeof(ApplicationAssemblyMarker).Assembly, typeof(PresentationAssemblyMarker).Assembly],
	includeInternalTypes: true);

builder.Services.AddSerilog((services, configure) => configure.ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(services));

builder.Services.AddOptions<UploadMediaOptions>().BindConfiguration(ConfigurationKeys.MediaUpload).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<UploadMediaOptions>(sp => sp.GetRequiredService<IOptions<UploadMediaOptions>>().Value);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseStatusCodePages();

var apiGroup = app.MapGroup("api");
apiGroup.MapPictureEndpoints();

app.UseHttpsRedirection();

await PersistenceHelper.AwaitDatabaseConnectionAsync(app.Services);
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
	await app.MigrateDatabaseAsync();
}

await app.RunAsync();
