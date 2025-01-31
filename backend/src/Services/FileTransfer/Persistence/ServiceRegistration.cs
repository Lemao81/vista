using Application.Abstractions;
using Domain.Media;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Extensions;
using Persistence.Interceptors;
using Persistence.Media;
using Persistence.Utilities;

namespace Persistence;

public static class ServiceRegistration
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<FileTransferDbContext>((sp, dbContextOptions) =>
		{
			var scope = sp.CreateScope();
			dbContextOptions.UseNpgsql(PersistenceHelper.CreateDataSource(configuration, DbNames.FileTransfer),
					npgsqlOptions =>
					{
						npgsqlOptions.SetPostgresVersion(17, 2);
						npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", DbSchemas.FileTransfer);
					})
				.UseSnakeCaseNamingConvention()
				.UseExceptionProcessor()
				.AddInterceptors(scope.ServiceProvider.GetRequiredService<AuditDateSaveChangesInterceptor>(),
					scope.ServiceProvider.GetRequiredService<DomainEventSaveChangesInterceptor>());
		});

		services.AddMinio(configuration);

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IMediaFolderRepository, MediaFolderRepository>();
		services.AddScoped<IObjectStorage, MinioObjectStorage>();
		services.AddScoped<AuditDateSaveChangesInterceptor>();
		services.AddScoped<DomainEventSaveChangesInterceptor>();

		return services;
	}
}
