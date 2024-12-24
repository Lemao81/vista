using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
	{
		return services;
	}
}
