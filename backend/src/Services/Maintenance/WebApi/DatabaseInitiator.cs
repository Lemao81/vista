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
		var connectionString = PersistenceHelper.CreateDataSource(configuration, database, true).ConnectionString;
		EnsureDatabase.For.PostgresqlDatabase(connectionString);
		var engine = GetUpgradeEngine(connectionString, database);
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
