﻿using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
			config.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
			config.AddOpenBehavior(typeof(LoggingBehavior<,>));
			config.AddOpenBehavior(typeof(TransactionalBehavior<,>));
			config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>();
			config.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
			config.AddOpenBehavior(typeof(TransactionalPipelineBehavior<,>));
		});

		return services;
	}
}
