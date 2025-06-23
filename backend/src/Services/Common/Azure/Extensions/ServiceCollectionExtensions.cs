using Azure.Core.Pipeline;
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
				.ConfigureOptions(o => o.Transport = new HttpClientTransport(
					                       new HttpClient(
						                       new HttpClientHandler
						                       {
#pragma warning disable S4830
#pragma warning disable MA0039
							                       ServerCertificateCustomValidationCallback =
#pragma warning restore MA0039
#pragma warning restore S4830
								                       HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
						                       })));

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
						ExcludeSharedTokenCacheCredential   = true,
						ExcludeVisualStudioCodeCredential   = true,
					}));
		});

		return services;
	}
}
