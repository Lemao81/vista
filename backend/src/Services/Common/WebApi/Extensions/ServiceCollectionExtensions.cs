using System.Text;
using Common.Application.Constants;
using Common.Presentation.Constants;
using Common.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

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

	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions<JwtBearerOptions>().BindConfiguration(ConfigurationKeys.Jwt).ValidateDataAnnotations().ValidateOnStart();

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters.ValidIssuer =
					configuration[ConfigurationKeys.JwtIssuer] ?? throw new MissingConfigurationException(ConfigurationKeys.JwtIssuer);

				options.TokenValidationParameters.ValidAudience =
					configuration[ConfigurationKeys.JwtAudience] ?? throw new MissingConfigurationException(ConfigurationKeys.JwtAudience);

				var secretKey = configuration[ConfigurationKeys.JwtSecretKey] ?? throw new MissingConfigurationException(ConfigurationKeys.JwtSecretKey);
				options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			});

		services.AddAuthorization();

		return services;
	}
}
