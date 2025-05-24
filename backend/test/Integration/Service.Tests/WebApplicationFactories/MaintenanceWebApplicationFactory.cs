using Common.WebApi.Constants;
using DotNet.Testcontainers.Containers;
using Maintenance.WebApi;
using Microsoft.AspNetCore.Hosting;
using Service.Tests.Abstractions;
using Testcontainers.Azurite;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;

namespace Service.Tests.WebApplicationFactories;

public class MaintenanceWebApplicationFactory : WebApplicationFactoryBase<WebApiAssemblyMarker>
{
	private readonly PostgreSqlContainer _postgresContainer;
	private readonly MinioContainer      _minioContainer;
	private readonly AzuriteContainer    _azuriteContainer;

	public MaintenanceWebApplicationFactory()
	{
		_postgresContainer = ContainerFactory.CreatePostgresContainer();
		_minioContainer    = ContainerFactory.CreateMinioContainer();
		_azuriteContainer  = ContainerFactory.CreateAzuriteContainer();
	}

	protected override IEnumerable<IContainer?> Containers => [_postgresContainer, _minioContainer];

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);

		Environment.SetEnvironmentVariable(EnvironmentVariableNames.InitiatePostgresDatabase, "true");
		Environment.SetEnvironmentVariable(EnvironmentVariableNames.InitiateMinio, "true");
		Environment.SetEnvironmentVariable(EnvironmentVariableNames.InitiateAzureBlobStorage, "true");

		UsePostgresDatabaseSetting(builder, _postgresContainer);
		UseMinioSetting(builder, _minioContainer);
		UseAzuriteSetting(builder, _azuriteContainer);
	}

	protected override async Task DoInitializeAsync()
	{
		await Task.WhenAll(_postgresContainer.StartAsync(), _minioContainer.StartAsync(), _azuriteContainer.StartAsync());
	}

	public new async Task DisposeAsync()
	{
		await base.DisposeAsync();

		await Task.WhenAll(_postgresContainer.DisposeAsync().AsTask(), _minioContainer.DisposeAsync().AsTask(), _azuriteContainer.DisposeAsync().AsTask());
	}
}
