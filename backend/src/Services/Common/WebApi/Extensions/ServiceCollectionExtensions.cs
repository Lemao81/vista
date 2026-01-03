using System.Text;
using Common.Application.Constants;
using Common.Domain.Authentication;
using Common.Presentation.Constants;
using Common.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

		services.AddCors(options => options.AddDefaultPolicy(p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()));

		return services;
	}

	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions<JwtOptions>().BindConfiguration(ConfigurationKeys.Jwt).ValidateDataAnnotations().ValidateOnStart();
		services.AddSingleton<JwtOptions>(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);

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
