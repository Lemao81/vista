using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
			config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
		});

		return services;
	}
}
