using Common.Domain.Storage;
using Common.Persistence;
using Common.Persistence.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Args;
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

	[Fact]
	public async Task When_startup_service_should_execute_filetransfer_migrations()
	{
		// Act + Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		await TestHelper.AwaitHealthiness(_webApplicationFactory);
		var migrationIds  = new[] { "20250128134342_Initial" };
		var configuration = _webApplicationFactory.Services.GetRequiredService<IConfiguration>();

		await using var dataSource = PersistenceHelper.CreateDataSource(configuration, DbNames.FileTransfer, persistSecurityInfo: true);
		await using var connection = new NpgsqlConnection(dataSource.ConnectionString);
		await connection.OpenAsync();

		await using var command = connection.CreateCommand();
		command.CommandText = "SELECT migration_id FROM vista_file_transfer.filetransfer.__ef_migrations_history";
		var reader             = await command.ExecuteReaderAsync();
		var actualMigrationIds = new List<string>();
		while (await reader.ReadAsync())
		{
			actualMigrationIds.Add(reader.GetString(0));
		}

		// Assert
		Assert.Equal(migrationIds.Length, actualMigrationIds.Count);
		Assert.True(migrationIds.All(id => actualMigrationIds.Contains(id, StringComparer.OrdinalIgnoreCase)));
	}

	[Fact]
	public async Task When_startup_service_should_create_minio_buckets()
	{
		// Act + Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		await TestHelper.AwaitHealthiness(_webApplicationFactory);
		var buckets     = new[] { Buckets.Media };
		var minioClient = _webApplicationFactory.Services.GetRequiredService<IMinioClient>();

		foreach (var bucket in buckets)
		{
			var exists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));

			// Assert
			Assert.True(exists);
		}
	}
}
