using Application;
using Application.Abstractions;
using Domain;
using Domain.Media;
using FileTransfer.Persistence;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Presentation;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddValidatorsFromAssemblyContaining<CommonPresentationAssemblyMarker>(includeInternalTypes: true);

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();

builder.Services.AddScoped<IMediaFolderRepository, AzureMediaFolderRepository>();
builder.Services.AddScoped<IObjectStorage, AzureObjectStorage>();
builder.Services.AddScoped<IUnitOfWork, AzureUnitOfWork>();

builder.Services.AddOptions<UploadMediaOptions>().BindConfiguration(ConfigurationKeys.MediaUpload).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<UploadMediaOptions>(sp => sp.GetRequiredService<IOptions<UploadMediaOptions>>().Value);

builder.Build().Run();
