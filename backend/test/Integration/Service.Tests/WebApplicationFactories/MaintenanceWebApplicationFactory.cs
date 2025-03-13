using DotNet.Testcontainers.Containers;
using Maintenance.WebApi;
using Microsoft.AspNetCore.Hosting;
using Service.Tests.Abstractions;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;

namespace Service.Tests.WebApplicationFactories;

public class MaintenanceWebApplicationFactory : WebApplicationFactoryBase<WebApiAssemblyMarker>
{
	private readonly PostgreSqlContainer _postgresContainer;
	private readonly MinioContainer      _minioContainer;

	public MaintenanceWebApplicationFactory()
	{
		_postgresContainer = ContainerFactory.CreatePostgresContainer();
		_minioContainer    = ContainerFactory.CreateMinioContainer();
	}

	protected override IEnumerable<IContainer?> Containers => [_postgresContainer, _minioContainer];

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);

		Environment.SetEnvironmentVariable(EnvironmentVariableNames.InitiatePostgresDatabase, "true");
		Environment.SetEnvironmentVariable(EnvironmentVariableNames.InitiateMinio, "true");

		UsePostgresDatabaseSetting(builder, _postgresContainer);
		UseMinioSetting(builder, _minioContainer);
	}

	protected override async Task DoInitializeAsync()
	{
		await Task.WhenAll(_postgresContainer.StartAsync(), _minioContainer.StartAsync());
	}

	public new async Task DisposeAsync()
	{
		await base.DisposeAsync();

		await _postgresContainer.DisposeAsync();
		await _minioContainer.DisposeAsync();
	}
}
