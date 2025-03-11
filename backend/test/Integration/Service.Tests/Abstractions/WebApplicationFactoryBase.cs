using Common.Application;
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
using Xunit.Abstractions;

namespace Service.Tests.Abstractions;

public abstract class WebApplicationFactoryBase<T> : WebApplicationFactory<T>, IAsyncLifetime where T : class
{
	private readonly DelegatingTestOutputHelper _delegatingTestOutputHelper;

	protected WebApplicationFactoryBase()
	{
		Network                     = new NetworkBuilder().WithName($"{GetType().Name}_{Guid.NewGuid()}").Build();
		ContainerFactory            = new ContainerFactory(Network);
		_delegatingTestOutputHelper = new DelegatingTestOutputHelper(() => TestOutputHelper);
	}

	protected INetwork         Network          { get; }
	protected ContainerFactory ContainerFactory { get; }

	protected abstract IEnumerable<IContainer?> Containers { get; }

	public ITestOutputHelper? TestOutputHelper { get; set; }

	public async Task InitializeAsync()
	{
		try
		{
			await DoInitializeAsync();
		}
		catch (Exception)
		{
			await LogExitedContainersAsync(Containers);

			throw;
		}
	}

	public new async Task DisposeAsync()
	{
		await Network.DisposeAsync();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Production");
		builder.ConfigureLogging(logBuilder =>
		{
			logBuilder.ClearProviders();
			logBuilder.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_delegatingTestOutputHelper));
		});
	}

	protected virtual Task DoInitializeAsync() => Task.CompletedTask;

	protected void UsePostgresDatabaseSetting(IWebHostBuilder builder, PostgreSqlContainer postgresContainer)
	{
		builder.UseSetting(ConfigurationKeys.DatabaseHost, $"{postgresContainer.Hostname}:{postgresContainer.GetMappedPublicPort(5432)}");
		builder.UseSetting(ConfigurationKeys.DatabaseUsername, "sa");
		builder.UseSetting(ConfigurationKeys.DatabasePassword, "adminpwd");
	}

	protected void UseMinioSetting(IWebHostBuilder builder, MinioContainer minioContainer)
	{
		builder.UseSetting(ConfigurationKeys.MinioEndpoint, $"{minioContainer.Hostname}:{minioContainer.GetMappedPublicPort(9000)}");
		builder.UseSetting(ConfigurationKeys.MinioAccessKey, "admin");
		builder.UseSetting(ConfigurationKeys.MinioSecretKey, "adminpwd");
	}

	private static async Task LogExitedContainersAsync(IEnumerable<IContainer?> containers)
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
