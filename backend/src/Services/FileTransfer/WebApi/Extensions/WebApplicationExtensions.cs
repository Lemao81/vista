using Microsoft.EntityFrameworkCore;
using Persistence;
using Polly;
using Polly.Retry;

namespace WebApi.Extensions;

public static class WebApplicationExtensions
{
	public static async Task AwaitDatabaseConnectionAsync(this WebApplication app)
	{
		using var scope     = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var       dbContext = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		var       logger    = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

		var pipeline = new ResiliencePipelineBuilder().AddRetry(new RetryStrategyOptions
			{
				Delay            = TimeSpan.FromSeconds(1),
				BackoffType      = DelayBackoffType.Exponential,
				MaxDelay         = TimeSpan.FromSeconds(10),
				MaxRetryAttempts = int.MaxValue,
				ShouldHandle     = new PredicateBuilder().Handle<Exception>(exception => exception is not OperationCanceledException)
			})
			.Build();

		await pipeline.ExecuteAsync(async (context, ct) =>
			{
				await context.Database.OpenConnectionAsync(cancellationToken: ct);
				logger.LogInformation("Database connection established");
			},
			dbContext);
	}

	public static async Task MigrateDatabaseAsync(this WebApplication app)
	{
		using var scope     = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var       dbContext = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		await dbContext.Database.MigrateAsync();
	}
}
