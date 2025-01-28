﻿using Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Infrastructure;

public static class ServiceRegistration
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
	                                                           IWebHostEnvironment     environment,
	                                                           ILoggingBuilder         loggingBuilder)
	{
		// TODO solve serilog 'inhibition'
		loggingBuilder.AddOpenTelemetry(logging =>
		{
			logging.IncludeFormattedMessage = true;
			logging.IncludeScopes           = true;
		});

		services.AddOpenTelemetry()
			.ConfigureResource(r => r.AddService(ServiceNames.FileTransfer))
			.WithMetrics(metrics =>
			{
				metrics.AddAspNetCoreInstrumentation();
				metrics.AddHttpClientInstrumentation();
				metrics.AddRuntimeInstrumentation();
				metrics.AddMeter(MeterNames.FileTransfer);
			})
			.WithTracing(tracing =>
			{
				if (environment.IsDevelopment())
				{
					tracing.SetSampler<AlwaysOnSampler>();
				}

				tracing.AddAspNetCoreInstrumentation();
				tracing.AddHttpClientInstrumentation();
				tracing.AddEntityFrameworkCoreInstrumentation();
			})
			.UseOtlpExporter();

		return services;
	}
}
