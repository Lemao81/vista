using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Persistence;
using Persistence.Utilities;
using Polly;
using Polly.Retry;

namespace WebApi.Extensions;

public static partial class WebApplicationExtensions
{
	[GeneratedRegex("Database=(.*?);")]
	private static partial Regex DatabaseRegex();

	public static async Task AwaitDatabaseConnectionAsync(this WebApplication app, string database)
	{
		await using var scope         = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
		var             configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
		var             logger        = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

		var connectionString = PersistenceHelper.CreateDataSource(configuration, database, true).ConnectionString;
		connectionString = DatabaseRegex().Replace(connectionString, "");
		var connection = new NpgsqlConnection(connectionString);

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

	public static async Task MigrateDatabaseAsync(this WebApplication app)
	{
		await using var scope     = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
		await using var dbContext = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		await dbContext.Database.MigrateAsync();
	}
}
