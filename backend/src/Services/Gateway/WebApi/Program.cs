using Common.Application.Constants;
using Common.Infrastructure.Extensions;
using Common.WebApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddOpenApi();

builder.Services.AddCommonServices();
builder.Services.AddTelemetry(builder.Environment, builder.Logging, ServiceNames.Gateway, MeterNames.Gateway);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection(ConfigurationKeys.ReverseProxy));

builder.Services.AddSerilog((sp, configuration) => configuration.ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(sp));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapReverseProxy();

await app.RunAsync();
