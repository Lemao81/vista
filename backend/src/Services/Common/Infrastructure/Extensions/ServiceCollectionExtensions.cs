using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Common.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTelemetry(
		this IServiceCollection services,
		IWebHostEnvironment     environment,
		ILoggingBuilder         loggingBuilder,
		string                  serviceName,
		string                  meterName)
	{
		loggingBuilder.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes           = true;
		});

		services.AddOpenTelemetry()
			.ConfigureResource(r => r.AddService(serviceName))
			.WithMetrics(metrics =>
			{
				metrics.AddAspNetCoreInstrumentation();
				metrics.AddHttpClientInstrumentation();
				metrics.AddRuntimeInstrumentation();
				metrics.AddMeter(meterName);
			})
			.WithTracing(tracing =>
			{
				if (environment.IsDevelopment())
				{
					tracing.SetSampler<AlwaysOnSampler>();
				}

				tracing.AddAspNetCoreInstrumentation();
				tracing.AddHttpClientInstrumentation();
				tracing.AddNpgsql();
			})
			.UseOtlpExporter();

		return services;
	}
}
