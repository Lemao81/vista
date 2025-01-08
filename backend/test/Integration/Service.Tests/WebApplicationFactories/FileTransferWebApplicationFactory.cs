using Application;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service.Tests.Utilities;
using Testcontainers.PostgreSql;
using WebApi;
using Xunit.Abstractions;

namespace Service.Tests.WebApplicationFactories;

public class FileTransferWebApplicationFactory : WebApplicationFactory<WebApiAssemblyMarker>, IAsyncLifetime
{
	private readonly INetwork                   _network;
	private readonly PostgreSqlContainer        _postgresContainer;
	private readonly DelegatingTestOutputHelper _delegatingTestOutputHelper;

	public FileTransferWebApplicationFactory()
	{
		_network                    = new NetworkBuilder().WithName($"{GetType().Name}_{Guid.NewGuid()}").Build();
		_postgresContainer          = CreatePostgresContainer();
		_delegatingTestOutputHelper = new DelegatingTestOutputHelper(() => TestOutputHelper);
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Staging");
		builder.ConfigureLogging(logBuilder =>
		{
			logBuilder.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_delegatingTestOutputHelper));
		});

		builder.UseSetting(ConfigurationKeys.DatabaseHost, $"{_postgresContainer.Hostname}:{_postgresContainer.GetMappedPublicPort(5432)}");
		builder.UseSetting(ConfigurationKeys.DatabaseUsername, "sa");
		builder.UseSetting(ConfigurationKeys.DatabasePassword, "test_pwd");
	}

	public ITestOutputHelper? TestOutputHelper { get; set; }

	public async Task InitializeAsync()
	{
		await _postgresContainer.StartAsync();
	}

	public new Task DisposeAsync() => Task.CompletedTask;

	private PostgreSqlContainer CreatePostgresContainer()
	{
		var image = IsLocal() ? "backend-vista-postgres" : "ghcr.io/lemao81/vista-postgres";

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
