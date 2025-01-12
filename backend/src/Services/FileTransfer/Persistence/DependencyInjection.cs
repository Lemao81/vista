using Application.Abstractions;
using Domain.Media;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Extensions;
using Persistence.Media;
using Persistence.Utilities;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<FileTransferDbContext>(dbContextOptions =>
		{
			dbContextOptions.UseNpgsql(PersistenceHelper.CreateDataSource(configuration),
					npgsqlOptions =>
					{
						npgsqlOptions.SetPostgresVersion(17, 2);
						npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", DbSchemas.FileTransfer);
					})
				.UseSnakeCaseNamingConvention()
				.UseExceptionProcessor();
		});

		services.AddMinio(configuration);

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IMediaFolderRepository, MediaFolderRepository>();
		services.AddScoped<IObjectStorage, MinioObjectStorage>();

		return services;
	}
}
