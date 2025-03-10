using Common.Application;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Maintenance.WebApi;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;

namespace Service.Tests.Utilities;

public class ContainerFactory
{
	private const string DatabaseUsername = "sa";
	private const string DatabasePassword = "adminpwd";
	private const string MinioUsername    = "admin";
	private const string MinioPassword    = "adminpwd";
	public const  string RemoteRepository = "ghcr.io/lemao81";

	private readonly INetwork _network;

	public ContainerFactory(INetwork network)
	{
		_network = network;
	}

	public PostgreSqlContainer CreatePostgresContainer()
	{
		var image = IsLocal() ? "backend-vista-postgres" : $"{RemoteRepository}/vista-postgres";

		return new PostgreSqlBuilder().WithImage(image)
			.WithName($"{NetworkAliases.Postgres}_{Guid.NewGuid()}")
			.WithNetwork(_network)
			.WithNetworkAliases(NetworkAliases.Postgres)
			.WithDatabase("sa")
			.WithUsername(DatabaseUsername)
			.WithPassword(DatabasePassword)
			.WithEnvironment("PGUSER", DatabaseUsername)
			// uncomment for local inspection
			// .WithPortBinding(5432)
			.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
			.Build();
	}

	public MinioContainer CreateMinioContainer()
	{
		var image = IsLocal() ? "backend-vista-minio" : $"{RemoteRepository}/vista-minio";

		return new MinioBuilder().WithImage(image)
			.WithName($"{NetworkAliases.Minio}_{Guid.NewGuid()}")
			.WithNetwork(_network)
			.WithNetworkAliases(NetworkAliases.Minio)
			.WithUsername(MinioUsername)
			.WithPassword(MinioPassword)
			.WithCommand("--console-address", ":9001")
			// uncomment for local inspection
			// .WithPortBinding(9001)
			.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
			.Build();
	}

	public IContainer CreateMaintenanceContainer()
	{
		var image = IsLocal() ? "backend-vista-maintenance-api" : $"{RemoteRepository}/vista-maintenance-api";

		return new ContainerBuilder().WithImage(image)
			.WithName($"{NetworkAliases.MaintenanceApi}_{Guid.NewGuid()}")
			.WithNetwork(_network)
			.WithNetworkAliases(NetworkAliases.MaintenanceApi)
			.WithEnvironment(ToEnvironmentName(ConfigurationKeys.DatabaseHost), $"{NetworkAliases.Postgres}:5432")
			.WithEnvironment(ToEnvironmentName(ConfigurationKeys.DatabaseUsername), DatabaseUsername)
			.WithEnvironment(ToEnvironmentName(ConfigurationKeys.DatabasePassword), DatabasePassword)
			.WithEnvironment(ToEnvironmentName(ConfigurationKeys.MinioEndpoint), $"{NetworkAliases.Minio}:9000")
			.WithEnvironment(ToEnvironmentName(ConfigurationKeys.MinioAccessKey), MinioUsername)
			.WithEnvironment(ToEnvironmentName(ConfigurationKeys.MinioSecretKey), MinioPassword)
			.WithEnvironment(EnvironmentVariableNames.InitiatePostgresDatabase, "true")
			.WithEnvironment(EnvironmentVariableNames.InitiateMinio, "true")
			.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
			.Build();
	}

	private static bool IsLocal() => Environment.OSVersion.Platform != PlatformID.Unix;

	private static string ToEnvironmentName(string configurationKey) => configurationKey.Replace(":", "__", StringComparison.Ordinal);
}
