using Common.Presentation.Constants;
using Common.WebApi.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace Common.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCommonServices(this IServiceCollection services)
	{
		services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
		{
			context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
			if (context.HttpContext.Items.TryGetValue(HttpContextItemKeys.ErrorCode, out var code))
			{
				context.ProblemDetails.Extensions.TryAdd(ProblemDetailsExtensionKeys.ErrorCode, code);
			}
		});

		services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();

		return services;
	}
}
