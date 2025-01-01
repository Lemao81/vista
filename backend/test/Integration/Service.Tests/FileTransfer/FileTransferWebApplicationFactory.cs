using Application;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using WebApi;

namespace Service.Tests.FileTransfer;

public class FileTransferWebApplicationFactory : WebApplicationFactory<WebApiAssemblyMarker>, IAsyncLifetime
{
	private readonly INetwork            _network;
	private readonly PostgreSqlContainer _postgresContainer;

	public FileTransferWebApplicationFactory()
	{
		_network           = new NetworkBuilder().WithName($"{GetType().Name}_{Guid.NewGuid()}").Build();
		_postgresContainer = CreatePostgresContainer();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseSetting(ConfigurationKeys.DatabaseHost, $"{_postgresContainer.Hostname}:{_postgresContainer.GetMappedPublicPort(5432)}");
		builder.UseSetting(ConfigurationKeys.DatabaseUsername, "sa");
		builder.UseSetting(ConfigurationKeys.DatabasePassword, "test_pwd");
	}

	public async Task InitializeAsync()
	{
		await _postgresContainer.StartAsync();
	}

	public new Task DisposeAsync() => Task.CompletedTask;

	private PostgreSqlContainer CreatePostgresContainer()
	{
		// TODO add remote image name
		var image = IsLocal() ? "backend-vista-postgres" : "";

		return new PostgreSqlBuilder().WithImage(image)
			.WithName($"postgres_{Guid.NewGuid()}")
			.WithNetwork(_network)
			.WithNetworkAliases(NetworkAliases.Postgres)
			// uncomment for local inspection
			// .WithPortBinding(5432, 5432)
			.WithDatabase("sa")
			.WithUsername("sa")
			.WithPassword("test_pwd")
			.WithEnvironment("PGUSER", "sa")
			.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
			.Build();
	}

	private static bool IsLocal() => Environment.OSVersion.Platform != PlatformID.Unix;
}
