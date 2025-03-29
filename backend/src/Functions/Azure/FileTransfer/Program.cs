using Azure.Identity;
using Common.Application;
using Common.Presentation;
using Common.WebApi;
using Common.WebApi.Extensions;
using FileTransfer.Application;
using FileTransfer.Domain;
using FileTransfer.Domain.Media;
using FileTransfer.Persistence;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedKernel;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Configuration.AddCommonAppSettings();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddValidatorsFromAssemblyContaining<CommonPresentationAssemblyMarker>(includeInternalTypes: true);

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddDatabasePersistenceServices(builder.Configuration);

builder.Services.AddAzureClients(clientBuilder =>
{
	var storageConnectionString = Environment.GetEnvironmentVariable(EnvironmentVariableNames.AzureWebJobsStorage) ??
	                              throw new MissingConfigurationException("AzureWebJobsStorage");

	// TODO use real client id
	var credentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = Guid.NewGuid().ToString() });
	clientBuilder.AddBlobServiceClient(new Uri(storageConnectionString), credentials);
});

builder.Services.AddScoped<IObjectStorage, AzureObjectStorage>();

builder.Services.AddOptions<UploadMediaOptions>().BindConfiguration(ConfigurationKeys.MediaUpload).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<UploadMediaOptions>(sp => sp.GetRequiredService<IOptions<UploadMediaOptions>>().Value);

builder.Build().Run();
