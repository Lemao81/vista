using Common.Application.Constants;
using Common.Azure.Extensions;
using Common.Presentation;
using Common.WebApi.Extensions;
using FileTransfer.Application;
using FileTransfer.Domain;
using FileTransfer.Domain.Media;
using FileTransfer.Infrastructure;
using FileTransfer.Persistence;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Configuration.AddCommonAppSettings();

//// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
//// builder.Services
////     .AddApplicationInsightsTelemetryWorkerService()
////     .ConfigureFunctionsApplicationInsights();

builder.Services.AddValidatorsFromAssemblyContaining<ICommonPresentationAssemblyMarker>(includeInternalTypes: true);

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddDatabasePersistenceServices(builder.Configuration);
builder.Services.AddAzureClients(builder.Configuration);

builder.Services.AddScoped<IObjectStorage, AzureObjectStorage>();

builder.Services.AddOptions<UploadMediaOptions>().BindConfiguration(ConfigurationKeys.MediaUpload).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<UploadMediaOptions>(sp => sp.GetRequiredService<IOptions<UploadMediaOptions>>().Value);

await builder.Build().RunAsync();
