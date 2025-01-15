using WebApi;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddOpenApi();

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
	var successful = DatabaseInitiator.UpgradeDatabase(app.Configuration);
	if (successful)
	{
		HealthCheck.IsHealthy = true;
	}
}
catch (Exception exception)
{
	Console.WriteLine(exception);
}

app.Run();
