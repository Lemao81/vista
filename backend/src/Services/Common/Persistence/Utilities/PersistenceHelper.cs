using Common.Application;
using Lemao.UtilExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Polly.Retry;
using SharedKernel;

namespace Common.Persistence.Utilities;

public static class PersistenceHelper
{
	public static NpgsqlDataSource CreateDataSource(IConfiguration configuration, string database, bool persistSecurityInfo = false)
	{
		var host = configuration[ConfigurationKeys.DatabaseHost];
		if (host.IsNullOrWhiteSpace())
		{
			host = Constants.DefaultDatabaseHost;
		}

		var username = configuration[ConfigurationKeys.DatabaseUsername];
		if (username.IsNullOrWhiteSpace())
		{
			throw new MissingConfigurationException("Database username is not configured");
		}

		var password = configuration[ConfigurationKeys.DatabasePassword];
		if (password.IsNullOrWhiteSpace())
		{
			var passwordFile = configuration[ConfigurationKeys.DatabasePasswordFile];
			if (!passwordFile.IsNullOrWhiteSpace() && File.Exists(passwordFile))
			{
				password = File.ReadAllText(passwordFile);
			}
		}

		if (password.IsNullOrWhiteSpace())
		{
			throw new MissingConfigurationException("Database password is not configured");
		}

		var builder = new NpgsqlDataSourceBuilder
		{
			ConnectionStringBuilder =
			{
				Host                = host,
				Database            = database,
				Username            = username,
				Password            = password,
				PersistSecurityInfo = persistSecurityInfo
			}
		};

		return builder.Build();
	}

	public static async Task AwaitDatabaseConnectionAsync(IServiceProvider serviceProvider, ILogger logger)
	{
		await using var scope         = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
		var             configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

		await using var dataSource = CreateDataSource(configuration, "", true);
		await using var connection = new NpgsqlConnection(dataSource.ConnectionString);

		var pipeline = new ResiliencePipelineBuilder().AddRetry(new RetryStrategyOptions
			{
				Delay            = TimeSpan.FromSeconds(1),
				BackoffType      = DelayBackoffType.Exponential,
				MaxDelay         = TimeSpan.FromSeconds(10),
				MaxRetryAttempts = int.MaxValue,
				ShouldHandle     = new PredicateBuilder().Handle<Exception>(exception => exception is not OperationCanceledException),
				OnRetry = _ =>
				{
					logger.LogInformation("Failed to establish database connection");

					return ValueTask.CompletedTask;
				}
			})
			.Build();

		await pipeline.ExecuteAsync(async (conn, ct) =>
			{
				await conn.OpenAsync(ct);
				logger.LogInformation("Database connection established");
				await conn.CloseAsync();
				await conn.DisposeAsync();
			},
			connection);
	}
}
