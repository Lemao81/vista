using Common.Application.Abstractions;
using Common.Application.Constants;
using Common.Persistence.Constants;
using Common.Persistence.Extensions;
using Common.Persistence.Interceptors;
using Common.Persistence.Utilities;
using EntityFramework.Exceptions.PostgreSQL;
using FileTransfer.Domain.Media;
using FileTransfer.Infrastructure.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace FileTransfer.Infrastructure;

public static class ServiceRegistration
{
	public static IServiceCollection AddInfrastructureServices(
		this IServiceCollection services,
		IWebHostEnvironment     environment,
		ILoggingBuilder         loggingBuilder)
	{
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
				tracing.AddNpgsql();
			})
			.UseOtlpExporter();

		return services;
	}

	public static IServiceCollection AddDatabasePersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<FileTransferDbContext>((sp, dbContextOptions) =>
		{
			var scope = sp.CreateScope();
			dbContextOptions.UseNpgsql(
					PersistenceHelper.CreateDataSource(configuration, DbNames.FileTransfer),
					npgsqlOptions =>
					{
						npgsqlOptions.SetPostgresVersion(17, 2);
						npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", DbSchemas.FileTransfer);
					})
				.UseSnakeCaseNamingConvention()
				.UseExceptionProcessor()
				.AddInterceptors(
					scope.ServiceProvider.GetRequiredService<AuditDateSaveChangesInterceptor>(),
					scope.ServiceProvider.GetRequiredService<DomainEventSaveChangesInterceptor>());
		});

		services.AddScoped<AuditDateSaveChangesInterceptor>();
		services.AddScoped<DomainEventSaveChangesInterceptor>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IMediaFolderRepository, MediaFolderRepository>();

		return services;
	}

	public static IServiceCollection AddObjectStoragePersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMinio(configuration);
		services.AddScoped<IObjectStorage, MinioObjectStorage>();

		return services;
	}
}
