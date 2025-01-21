using Persistence.Extensions;
using WebApi;
using WebApi.Extensions;
using WebApi.Initiators;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddScoped<IInitiator, DatabaseInitiator>();
builder.Services.AddScoped<IInitiator, MinioInitiator>();

builder.Services.AddOpenApi();

builder.Services.AddMinio(builder.Configuration);

builder.Services.AddHealthChecks().AddCheck<HealthCheck>("healthcheck");

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

try
{
	using var scope      = app.Services.CreateScope();
	var       initiators = scope.ServiceProvider.GetServices<IInitiator>();
	var       results    = await Task.WhenAll(initiators.Select(i => i.InitiateAsync()));
	if (results.All(r => r))
	{
		HealthCheck.IsHealthy = true;
	}
}
catch (Exception exception)
{
	Console.WriteLine(exception);
}

await app.RunAsync();
