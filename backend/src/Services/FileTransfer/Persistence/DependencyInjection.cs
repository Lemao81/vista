using Domain.Media;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Media;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
	{
		services.AddScoped<IMediaFolderRepository, MediaFolderRepository>();
		services.AddScoped<IObjectStorage, MinIoObjectStorage>();

		return services;
	}
}
