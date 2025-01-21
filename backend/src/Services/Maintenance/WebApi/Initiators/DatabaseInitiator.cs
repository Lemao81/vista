using DbUp;
using DbUp.Engine;
using Persistence;
using Persistence.Utilities;

namespace WebApi.Initiators;

internal sealed class DatabaseInitiator : IInitiator
{
	private readonly IConfiguration             _configuration;
	private readonly ILogger<DatabaseInitiator> _logger;

	public DatabaseInitiator(IConfiguration configuration, ILogger<DatabaseInitiator> logger)
	{
		_configuration = configuration;
		_logger        = logger;
	}

	public async Task<bool> InitiateAsync(CancellationToken cancellationToken = default)
	{
		var result = await UpgradeDatabaseAsync(DbNames.FileTransfer);

		return result;
	}

	private async Task<bool> UpgradeDatabaseAsync(string database)
	{
		await using var dataSource = PersistenceHelper.CreateDataSource(_configuration, database, true);
		EnsureDatabase.For.PostgresqlDatabase(dataSource.ConnectionString);
		var engine = GetUpgradeEngine(dataSource.ConnectionString, database);
		if (!engine.IsUpgradeRequired())
		{
			return true;
		}

		var result = engine.PerformUpgrade();
		if (result.Successful)
		{
			_logger.LogInformation("Database '{Database}' upgrade successful", database);

			return true;
		}

		_logger.LogInformation("Database '{Database}' upgrade failed: {Error}", database, result.Error);

		return false;
	}

	private static UpgradeEngine GetUpgradeEngine(string connectionString, string database) =>
		DeployChanges.To.PostgresqlDatabase(connectionString)
			.WithScriptsFromFileSystem(Path.Combine(AppContext.BaseDirectory, "DbScripts", database))
			.WithVariable("EF", "$$")
			.LogToConsole()
			.Build();
}
