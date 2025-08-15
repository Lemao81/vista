using System.Globalization;
using Common.Application.Constants;
using Common.Persistence.Utilities;
using Common.WebApi.Extensions;
using FileTransfer.Application;
using FileTransfer.Domain;
using FileTransfer.Domain.Media;
using FileTransfer.Infrastructure;
using FileTransfer.Presentation;
using FileTransfer.Presentation.Pictures;
using FileTransfer.WebApi.Extensions;
using Microsoft.Extensions.Options;
using Serilog;

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console(formatProvider: CultureInfo.InvariantCulture).CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddOpenApi();

builder.Services.AddCommonServices();
builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Environment, builder.Logging);
builder.Services.AddDatabasePersistenceServices(builder.Configuration);
builder.Services.AddObjectStoragePersistenceServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddSerilog((services, configure) => configure.ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(services));

builder.Services.AddOptions<UploadMediaOptions>().BindConfiguration(ConfigurationKeys.MediaUpload).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<UploadMediaOptions>(sp => sp.GetRequiredService<IOptions<UploadMediaOptions>>().Value);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseStatusCodePages();

var apiGroup = app.MapGroup("api");
apiGroup.MapPictureEndpoints();

app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
await PersistenceHelper.AwaitDatabaseConnectionAsync(app.Services, logger);
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
	await app.MigrateDatabaseAsync();
}

await app.RunAsync();
