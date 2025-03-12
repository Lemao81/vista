using Common.Persistence;
using Common.Persistence.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Service.Tests.Utilities;
using Service.Tests.WebApplicationFactories;
using Xunit.Abstractions;

namespace Service.Tests.Maintenance;

public class InitiationTests : IClassFixture<MaintenanceWebApplicationFactory>
{
	private readonly MaintenanceWebApplicationFactory _webApplicationFactory;
	private readonly ITestOutputHelper                _testOutputHelper;

	public InitiationTests(MaintenanceWebApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper)
	{
		_webApplicationFactory = webApplicationFactory;
		_testOutputHelper      = testOutputHelper;
	}

	[Fact]
	public async Task When_startup_service_should_create_databases()
	{
		// Act + Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		await TestHelper.AwaitHealthiness(_webApplicationFactory);
		var             databases     = new[] { DbNames.FileTransfer };
		var             configuration = _webApplicationFactory.Services.GetRequiredService<IConfiguration>();
		await using var dataSource    = PersistenceHelper.CreateDataSource(configuration, DbNames.Postgres, persistSecurityInfo: true);
		await using var connection    = new NpgsqlConnection(dataSource.ConnectionString);
		await connection.OpenAsync();
		foreach (var database in databases)
		{
			_testOutputHelper.WriteLine($"Checking for database {database}");
			await using var command = connection.CreateCommand();
			command.CommandText = "SELECT 1 FROM pg_database WHERE datname = @dbname";
			command.Parameters.AddWithValue("dbname", database);
			var result = await command.ExecuteScalarAsync();

			// Assert
			Assert.NotNull(result);
		}
	}
}
