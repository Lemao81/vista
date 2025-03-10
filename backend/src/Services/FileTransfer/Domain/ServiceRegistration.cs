using Microsoft.Extensions.DependencyInjection;

namespace FileTransfer.Domain;

public static class ServiceRegistration
{
	public static IServiceCollection AddDomainServices(this IServiceCollection services)
	{
		return services;
	}
}
