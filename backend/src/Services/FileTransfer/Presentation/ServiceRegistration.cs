﻿using Common.Presentation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FileTransfer.Presentation;

public static class ServiceRegistration
{
	public static IServiceCollection AddPresentationServices(this IServiceCollection services)
	{
		services.AddValidatorsFromAssemblies(
			[typeof(CommonPresentationAssemblyMarker).Assembly, typeof(PresentationAssemblyMarker).Assembly],
			includeInternalTypes: true);

		return services;
	}
}
