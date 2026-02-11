#if TEST
using Azure.Core.Pipeline;
#endif
using Azure.Identity;
using Common.Application.Constants;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Azure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddAzureClients(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAzureClients(clientBuilder =>
		{
			clientBuilder.AddBlobServiceClient(configuration.GetRequiredSection(ConfigurationKeys.AzureStorageBlob))
#if TEST
#pragma warning disable S4830
#pragma warning disable MA0039
				.ConfigureOptions(o => o.Transport = new HttpClientTransport(
					                       new HttpClient(
						                       new HttpClientHandler
						                       {
							                       ServerCertificateCustomValidationCallback =
								                       HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
						                       })))
#endif
				;

			var managedIdentityClientId = configuration.GetValue<string>(ConfigurationKeys.AzureStorageManagedIdentityClientId);
			clientBuilder.UseCredential(
				new DefaultAzureCredential(
					new DefaultAzureCredentialOptions
					{
						ManagedIdentityClientId             = managedIdentityClientId,
						ExcludeEnvironmentCredential        = true,
						ExcludeInteractiveBrowserCredential = true,
						ExcludeVisualStudioCredential       = true,
						ExcludeWorkloadIdentityCredential   = true,
						ExcludeAzureDeveloperCliCredential  = true,
						ExcludeAzurePowerShellCredential    = true,
						ExcludeVisualStudioCodeCredential   = true,
					}));
		});

		return services;
	}
}
