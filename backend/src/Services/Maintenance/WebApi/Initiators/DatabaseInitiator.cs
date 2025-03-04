using Common.Persistence;
using Common.Persistence.Utilities;
using DbUp;
using DbUp.Engine;
using Lemao.UtilExtensions;

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
		var result = await UpgradeDatabaseAsync(DbNames.FileTransfer, cancellationToken);

		return result;
	}

	private async Task<bool> UpgradeDatabaseAsync(string database, CancellationToken cancellationToken)
	{
		await using var dataSource = PersistenceHelper.CreateDataSource(_configuration, database, true);
		EnsureDatabase.For.PostgresqlDatabase(dataSource.ConnectionString);
		var engine = GetUpgradeEngine(dataSource.ConnectionString, database);
		if (!engine.IsUpgradeRequired())
		{
			return true;
		}

		cancellationToken.ThrowIfCancellationRequested();

		var scriptsToExecute = engine.GetScriptsToExecute().Select(s => s.Name);
		_logger.LogInformation("Scripts to execute: {Scripts}", scriptsToExecute.ToCommaSeparated());

		var result = engine.PerformUpgrade();
		if (result.Successful)
		{
			_logger.LogInformation("Database '{Database}' upgrade successful", database);

			return true;
		}

		_logger.LogInformation("Database '{Database}' upgrade failed: {Error}", database, result.Error);

		cancellationToken.ThrowIfCancellationRequested();

		return false;
	}

	private UpgradeEngine GetUpgradeEngine(string connectionString, string database) =>
		DeployChanges.To.PostgresqlDatabase(connectionString)
			.WithScriptsFromFileSystem(Path.Combine(AppContext.BaseDirectory, "DbScripts", database))
			.WithVariable("EF", "$$")
			.LogTo(new DbUpLogger(_logger))
			.Build();
}
