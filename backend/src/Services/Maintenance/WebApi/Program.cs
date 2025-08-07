using Common.Azure.Extensions;
using Common.Persistence.Extensions;
using Common.Persistence.Utilities;
using Common.WebApi.Constants;
using Common.WebApi.Extensions;
using Lemao.UtilExtensions;
using Maintenance.WebApi;
using Maintenance.WebApi.Abstractions;
using Maintenance.WebApi.Initiators;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddOpenApi();

builder.Services.AddHealthChecks().AddCheck<HealthCheck>("healthcheck");

var initiateDatabase = EnvironmentVariable.IsTrue(EnvironmentVariableNames.InitiatePostgresDatabase);
if (initiateDatabase)
{
	builder.Services.AddScoped<IInitiator, PostgresDatabaseInitiator>();
}

if (EnvironmentVariable.IsTrue(EnvironmentVariableNames.InitiateMinio))
{
	builder.Services.AddMinio(builder.Configuration);
	builder.Services.AddScoped<IInitiator, MinioInitiator>();
}

if (EnvironmentVariable.IsTrue(EnvironmentVariableNames.InitiateAzureBlobStorage))
{
	builder.Services.AddAzureClients(builder.Configuration);
	builder.Services.AddScoped<IInitiator, AzureBlobStorageInitiator>();
}

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
if (initiateDatabase)
{
	await PersistenceHelper.AwaitDatabaseConnectionAsync(app.Services, logger);
}

try
{
	await using var scope      = app.Services.CreateAsyncScope();
	var             initiators = scope.ServiceProvider.GetServices<IInitiator>().Where(i => i.IsEnabled()).ToList();
	if (initiators.Count == 0)
	{
		logger.LogInformation("No initiators to be executed");
		HealthCheck.IsHealthy = true;
	}
	else
	{
		logger.LogInformation("Initiators to be executed: {Initiators}", initiators.Select(i => i.GetType().Name).ToCommaSeparated());
		logger.LogInformation("Initiators start initiating");
		_ = Task.Run(async () =>
		{
			var results = await Task.WhenAll(initiators.Select(i => i.InitiateAsync()));
			if (results.All(r => r))
			{
				HealthCheck.IsHealthy = true;
				logger.LogInformation("All initiators initiated successfully");
			}
		});

		logger.LogInformation("Initiators finished initiating");
	}
}
catch (Exception exception)
{
	logger.LogError(exception);
}

await app.RunAsync();
