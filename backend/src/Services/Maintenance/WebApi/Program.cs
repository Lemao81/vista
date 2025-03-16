using Common.Persistence.Extensions;
using Common.Persistence.Utilities;
using Common.WebApi.Extensions;
using Lemao.UtilExtensions;
using Maintenance.WebApi;
using Maintenance.WebApi.Initiators;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddScoped<IInitiator, PostgresDatabaseInitiator>();
builder.Services.AddScoped<IInitiator, MinioInitiator>();

builder.Services.AddOpenApi();

if (EnvironmentVariable.IsTrue(EnvironmentVariableNames.InitiateMinio))
{
	builder.Services.AddMinio(builder.Configuration);
}

builder.Services.AddHealthChecks().AddCheck<HealthCheck>("healthcheck");

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
await PersistenceHelper.AwaitDatabaseConnectionAsync(app.Services, logger);

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
		_ = Task.Run(async () =>
		{
			var results = await Task.WhenAll(initiators.Select(i => i.InitiateAsync()));
			if (results.All(r => r))
			{
				HealthCheck.IsHealthy = true;
			}
		});
	}
}
catch (Exception exception)
{
	logger.LogError(exception);
}

await app.RunAsync();
