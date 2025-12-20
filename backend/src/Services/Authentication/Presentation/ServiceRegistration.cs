using Common.Presentation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Presentation;

public static class ServiceRegistration
{
	public static IServiceCollection AddPresentationServices(this IServiceCollection services)
	{
		services.AddValidatorsFromAssemblies(
			[typeof(ICommonPresentationAssemblyMarker).Assembly, typeof(IPresentationAssemblyMarker).Assembly],
			includeInternalTypes: true);

		return services;
	}
}
