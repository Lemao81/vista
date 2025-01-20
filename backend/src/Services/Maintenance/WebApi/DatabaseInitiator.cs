using DbUp;
using DbUp.Engine;
using Persistence;
using Persistence.Utilities;

namespace WebApi;

internal static class DatabaseInitiator
{
	public static bool UpgradeDatabase(IConfiguration configuration)
	{
		return UpgradeDatabase(configuration, DbNames.FileTransfer);
	}

	private static bool UpgradeDatabase(IConfiguration configuration, string database)
	{
		using var dataSource = PersistenceHelper.CreateDataSource(configuration, database, true);
		EnsureDatabase.For.PostgresqlDatabase(dataSource.ConnectionString);
		var engine = GetUpgradeEngine(dataSource.ConnectionString, database);
		if (!engine.IsUpgradeRequired())
		{
			return true;
		}

		var result = engine.PerformUpgrade();
		if (result.Successful)
		{
			return result.Successful;
		}

		Console.WriteLine($"'{database}' upgrade failed: {result.Error}");

		return false;
	}

	private static UpgradeEngine GetUpgradeEngine(string connectionString, string database) =>
		DeployChanges.To.PostgresqlDatabase(connectionString)
			.WithScriptsFromFileSystem(Path.Combine(AppContext.BaseDirectory, "DbScripts", database))
			.WithVariable("EF", "$$")
			.LogToConsole()
			.Build();
}
