using Application;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service.Tests.Utilities;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;
using WebApi;
using Xunit.Abstractions;

namespace Service.Tests.WebApplicationFactories;

public class FileTransferWebApplicationFactory : WebApplicationFactory<WebApiAssemblyMarker>, IAsyncLifetime
{
	private readonly INetwork                   _network;
	private readonly PostgreSqlContainer        _postgresContainer;
	private readonly MinioContainer             _minioContainer;
	private readonly DelegatingTestOutputHelper _delegatingTestOutputHelper;

	public FileTransferWebApplicationFactory()
	{
		_network                    = new NetworkBuilder().WithName($"{GetType().Name}_{Guid.NewGuid()}").Build();
		_postgresContainer          = CreatePostgresContainer();
		_minioContainer             = CreateMinioContainer();
		_delegatingTestOutputHelper = new DelegatingTestOutputHelper(() => TestOutputHelper);
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Staging");
		builder.ConfigureLogging(logBuilder =>
		{
			logBuilder.ClearProviders();
			logBuilder.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_delegatingTestOutputHelper));
		});

		builder.UseSetting(ConfigurationKeys.DatabaseHost, $"{_postgresContainer.Hostname}:{_postgresContainer.GetMappedPublicPort(5432)}");
		builder.UseSetting(ConfigurationKeys.DatabaseUsername, "sa");
		builder.UseSetting(ConfigurationKeys.DatabasePassword, "adminpwd");
		builder.UseSetting(ConfigurationKeys.MinioEndpoint, $"{_minioContainer.Hostname}:{_minioContainer.GetMappedPublicPort(9000)}");
		builder.UseSetting(ConfigurationKeys.MinioAccessKey, "admin");
		builder.UseSetting(ConfigurationKeys.MinioSecretKey, "adminpwd");
	}

	public ITestOutputHelper? TestOutputHelper { get; set; }

	public async Task InitializeAsync()
	{
		await Task.WhenAll(_postgresContainer.StartAsync(), _minioContainer.StartAsync());
	}

	public new async Task DisposeAsync()
	{
		await _network.DisposeAsync();
		await _postgresContainer.DisposeAsync();
		await _minioContainer.DisposeAsync();
	}

	private PostgreSqlContainer CreatePostgresContainer()
	{
		var image = IsLocal() ? "backend-vista-postgres" : "ghcr.io/lemao81/vista-postgres";

		return new PostgreSqlBuilder().WithImage(image)
			.WithName($"postgres_{Guid.NewGuid()}")
			.WithNetwork(_network)
			.WithNetworkAliases(NetworkAliases.Postgres)
			.WithDatabase("sa")
			.WithUsername("sa")
			.WithPassword("adminpwd")
			.WithEnvironment("PGUSER", "sa")
			// uncomment for local inspection
			// .WithPortBinding(5432)
			.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
			.Build();
	}

	private MinioContainer CreateMinioContainer()
	{
		var image = IsLocal() ? "backend-vista-minio" : "ghcr.io/lemao81/vista-minio";

		return new MinioBuilder().WithImage(image)
			.WithName($"minio_{Guid.NewGuid()}")
			.WithNetwork(_network)
			.WithNetworkAliases(NetworkAliases.Minio)
			.WithUsername("admin")
			.WithPassword("adminpwd")
			.WithCommand("--console-address", ":9001")
			// uncomment for local inspection
			// .WithPortBinding(9001)
			.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
			.Build();
	}

	private static bool IsLocal() => Environment.OSVersion.Platform != PlatformID.Unix;
}
