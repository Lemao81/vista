using Application;
using Domain.Abstractions;
using Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Persistence.Media;
using UtilExtensions;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContextPool<FileTransferDbContext>(dbContextOptions =>
		{
			dbContextOptions.UseNpgsql(CreateDataSource(configuration), npgsqlOptions =>
				{
					npgsqlOptions.SetPostgresVersion(17, 2);
					npgsqlOptions.MigrationsHistoryTable("__ef_migrations_history", DbSchemas.FileTransfer);
				})
				.UseSnakeCaseNamingConvention();
		});

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IMediaFolderRepository, MediaFolderRepository>();
		services.AddScoped<IObjectStorage, MinIoObjectStorage>();

		return services;
	}

	private static NpgsqlDataSource CreateDataSource(IConfiguration configuration)
	{
		var host = configuration[ConfigurationKeys.DbHost];
		if (host.IsNullOrWhiteSpace())
		{
			throw new ApplicationException("Database host is not configured");
		}

		var username = configuration[ConfigurationKeys.DbUsername];
		if (username.IsNullOrWhiteSpace())
		{
			throw new ApplicationException("Database username is not configured");
		}

		var password = configuration[ConfigurationKeys.DbPassword];
		if (password.IsNullOrWhiteSpace())
		{
			throw new ApplicationException("Database password is not configured");
		}

		var builder = new NpgsqlDataSourceBuilder
		{
			ConnectionStringBuilder =
			{
				Host     = host,
				Database = "vista_file_transfer",
				Username = username,
				Password = password
			}
		};

		return builder.Build();
	}
}
