using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public static class ServiceRegistration
{
	public static IServiceCollection AddPresentationServices(this IServiceCollection services)
	{
		return services;
	}
}
