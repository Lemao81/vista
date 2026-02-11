using DotNet.Testcontainers.Containers;
using FileTransfer.WebApi;
using Microsoft.AspNetCore.Hosting;
using Service.Tests.Abstractions;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;

namespace Service.Tests.WebApplicationFactories;

public class FileTransferWebApplicationFactory : WebApplicationFactoryBase<IWebApiAssemblyMarker>
{
	private readonly PostgreSqlContainer _postgresContainer;
	private readonly MinioContainer      _minioContainer;
	private          IContainer?         _maintenanceContainer;

	public FileTransferWebApplicationFactory()
	{
		_postgresContainer = ContainerFactory.CreatePostgresContainer();
		_minioContainer    = ContainerFactory.CreateMinioContainer();
	}

	protected override IEnumerable<IContainer?> Containers => [_postgresContainer, _minioContainer, _maintenanceContainer];

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);

		UsePostgresDatabaseSetting(builder, _postgresContainer);
		UseMinioSetting(builder, _minioContainer);
		UseJwtSetting(builder);
	}

	protected override async Task DoInitializeAsync()
	{
		await Task.WhenAll(_postgresContainer.StartAsync(), _minioContainer.StartAsync());
		_maintenanceContainer = ContainerFactory.CreateMaintenanceContainer(initiateMinio: true);
		await _maintenanceContainer.StartAsync();
	}

	public new async Task DisposeAsync()
	{
		await base.DisposeAsync();

		if (_maintenanceContainer is not null)
		{
			await _maintenanceContainer.DisposeAsync();
		}

		await _postgresContainer.DisposeAsync();
		await _minioContainer.DisposeAsync();
	}
}
