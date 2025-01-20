using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>();
			config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
		});

		return services;
	}
}
