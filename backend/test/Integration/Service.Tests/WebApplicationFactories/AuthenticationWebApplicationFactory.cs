using Authentication.WebApi;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Service.Tests.Abstractions;
using Testcontainers.PostgreSql;

namespace Service.Tests.WebApplicationFactories;

public class AuthenticationWebApplicationFactory : WebApplicationFactoryBase<IWebApiAssemblyMarker>
{
	private readonly PostgreSqlContainer _postgresContainer;
	private          IContainer?         _maintenanceContainer;

	public AuthenticationWebApplicationFactory()
	{
		_postgresContainer = ContainerFactory.CreatePostgresContainer();
	}

	protected override IEnumerable<IContainer?> Containers => [_postgresContainer, _maintenanceContainer];

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);

		UsePostgresDatabaseSetting(builder, _postgresContainer);
		UseJwtSetting(builder);
	}

	protected override async Task DoInitializeAsync()
	{
		await Task.WhenAll(_postgresContainer.StartAsync());
		_maintenanceContainer = ContainerFactory.CreateMaintenanceContainer();
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
	}
}
