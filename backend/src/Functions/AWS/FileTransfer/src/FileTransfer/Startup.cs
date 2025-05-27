using FileTransfer.Application;
using FileTransfer.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileTransfer;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
#pragma warning disable CA1822
#pragma warning disable S2325
	public void ConfigureServices(IServiceCollection services)
#pragma warning restore S2325
#pragma warning restore CA1822
	{
		var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true);

		var configuration = builder.Build();
		services.AddSingleton<IConfiguration>(configuration);

		services.AddDomainServices();
		services.AddApplicationServices();
	}
}
