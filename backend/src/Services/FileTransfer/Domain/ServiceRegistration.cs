using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class ServiceRegistration
{
	public static IServiceCollection AddDomainServices(this IServiceCollection services)
	{
		return services;
	}
}
