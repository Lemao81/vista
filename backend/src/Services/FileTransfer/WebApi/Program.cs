using Application;
using Domain;
using Domain.Media;
using FluentValidation;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;
using SharedKernel;
using WebApi;
using WebApi.Extensions;
using WebApi.Pictures;

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
	if (context.HttpContext.Items.TryGetValue(HttpContextItemKeys.ErrorCode, out var code) && code is ErrorCode errorCode)
	{
		context.ProblemDetails.Extensions.Add(ProblemDetailsExtensionKeys.ErrorCode, errorCode.Value);
	}
});

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddValidatorsFromAssemblies([typeof(WebApiAssemblyMarker).Assembly, typeof(ApplicationAssemblyMarker).Assembly], includeInternalTypes: true);
builder.Services.AddSerilog((services, configure) => configure.ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(services));

builder.Services.Configure<UploadMediaOptions>(builder.Configuration.GetSection(ConfigurationKeys.MediaUpload));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseStatusCodePages();
app.MapPictureEndpoints();
app.UseHttpsRedirection();

await app.AwaitDatabaseConnectionAsync();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
	await app.MigrateDatabaseAsync();
}

app.Run();
