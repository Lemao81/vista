using Common.Application.Abstractions;
using Common.Persistence.Constants;
using Common.Persistence.Extensions;
using Common.Persistence.Interceptors;
using Common.Persistence.Utilities;
using EntityFramework.Exceptions.PostgreSQL;
using FileTransfer.Domain.Media;
using FileTransfer.Persistence.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileTransfer.Persistence;

public static class ServiceRegistration
{
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
