using Common.Persistence.Constants;
using Common.Persistence.Utilities;
using Common.WebApi.Constants;
using DbUp;
using DbUp.Engine;
using Lemao.UtilExtensions;
using Maintenance.WebApi.Abstractions;

namespace Maintenance.WebApi.Initiators;

internal sealed class PostgresDatabaseInitiator : IInitiator
{
	private readonly IConfiguration                     _configuration;
	private readonly ILogger<PostgresDatabaseInitiator> _logger;

	public PostgresDatabaseInitiator(IConfiguration configuration, ILogger<PostgresDatabaseInitiator> logger)
	{
		_configuration = configuration;
		_logger        = logger;
	}

	public bool IsEnabled() => EnvironmentVariable.IsTrue(EnvironmentVariableNames.InitiatePostgresDatabase);

	public async Task<bool> InitiateAsync(CancellationToken cancellationToken = default)
	{
		var result = await UpgradeDatabaseAsync(DbNames.FileTransfer, cancellationToken);

		return result;
	}

	private async Task<bool> UpgradeDatabaseAsync(string database, CancellationToken cancellationToken)
	{
		await using var dataSource = PersistenceHelper.CreateDataSource(_configuration, database, persistSecurityInfo: true);
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

		_logger.LogWarning(result.Error, "Database '{Database}' upgrade failed: {Message}", database, result.Error.Message);

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
