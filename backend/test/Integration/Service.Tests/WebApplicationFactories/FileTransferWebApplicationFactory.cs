using Application;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Lemao.UtilExtensions;
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
	private readonly ContainerFactory           _containerFactory;
	private readonly PostgreSqlContainer        _postgresContainer;
	private readonly MinioContainer             _minioContainer;
	private          IContainer?                _maintenanceContainer;
	private readonly DelegatingTestOutputHelper _delegatingTestOutputHelper;

	public FileTransferWebApplicationFactory()
	{
		_network                    = new NetworkBuilder().WithName($"{GetType().Name}_{Guid.NewGuid()}").Build();
		_containerFactory           = new ContainerFactory(_network);
		_postgresContainer          = _containerFactory.CreatePostgresContainer();
		_minioContainer             = _containerFactory.CreateMinioContainer();
		_delegatingTestOutputHelper = new DelegatingTestOutputHelper(() => TestOutputHelper);
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Production");
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
		try
		{
			await Task.WhenAll(_postgresContainer.StartAsync(), _minioContainer.StartAsync());
			_maintenanceContainer = _containerFactory.CreateMaintenanceContainer();
			await _maintenanceContainer.StartAsync();
		}
		catch (Exception)
		{
			await LogExitedContainersAsync(_postgresContainer, _minioContainer, _maintenanceContainer);

			throw;
		}
	}

	public new async Task DisposeAsync()
	{
		if (_maintenanceContainer is not null)
		{
			await _maintenanceContainer.DisposeAsync();
		}

		await _postgresContainer.DisposeAsync();
		await _minioContainer.DisposeAsync();
		await _network.DisposeAsync();
	}

	private static async Task LogExitedContainersAsync(params IContainer?[] containers)
	{
		foreach (var container in containers.Where(c => c is not null && c.State == TestcontainersStates.Exited))
		{
			var stderr = (await container!.GetLogsAsync()).Stderr;
			if (!stderr.IsNullOrWhiteSpace())
			{
				Console.WriteLine(stderr);
			}
		}
	}
}
