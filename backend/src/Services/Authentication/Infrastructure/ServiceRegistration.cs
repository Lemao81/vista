using Common.Application.Abstractions;
using Common.Application.Constants;
using Common.Infrastructure.Extensions;
using Common.Persistence;
using Common.Persistence.Constants;
using Common.Persistence.Interceptors;
using Common.Persistence.Utilities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Authentication.Infrastructure;

public static class ServiceRegistration
{
	public static IServiceCollection AddInfrastructureServices(
		this IServiceCollection services,
		IWebHostEnvironment     environment,
		ILoggingBuilder         loggingBuilder)
	{
		services.AddTelemetry(environment, loggingBuilder, ServiceNames.Authentication, MeterNames.Authentication);

		return services;
	}

	public static IServiceCollection AddDatabasePersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<AuthenticationDbContext>((sp, dbContextOptions) =>
		{
			var scope = sp.CreateScope();
			dbContextOptions.UseNpgsql(
					PersistenceHelper.CreateDataSource(configuration, DbNames.Authentication),
					npgsqlOptions =>
					{
						npgsqlOptions.SetPostgresVersion(17, 2);
						npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", DbSchemas.Authentication);
					})
				.UseSnakeCaseNamingConvention()
				.UseExceptionProcessor()
				.AddInterceptors(
					scope.ServiceProvider.GetRequiredService<AuditDateSaveChangesInterceptor>(),
					scope.ServiceProvider.GetRequiredService<DomainEventSaveChangesInterceptor>());
		});

		services.AddScoped<AuditDateSaveChangesInterceptor>();
		services.AddScoped<DomainEventSaveChangesInterceptor>();
		services.AddScoped<IUnitOfWork, UnitOfWork<AuthenticationDbContext>>();

		return services;
	}
}
