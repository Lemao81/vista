using Common.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Application;

public static class ServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>(includeInternalTypes: true);

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblyContaining<IApplicationAssemblyMarker>();
			config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
		});

		return services;
	}
}
