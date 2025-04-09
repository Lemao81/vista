using Azure.Identity;
using Common.Application;
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
			clientBuilder.AddBlobServiceClient(configuration.GetRequiredSection(ConfigurationKeys.AzureStorageBlob));
			var managedIdentityClientId = configuration.GetValue<string>(ConfigurationKeys.AzureStorageManagedIdentityClientId);
			clientBuilder.UseCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
			{
				ManagedIdentityClientId             = managedIdentityClientId,
				ExcludeEnvironmentCredential        = true,
				ExcludeInteractiveBrowserCredential = true,
				ExcludeVisualStudioCredential       = true,
				ExcludeWorkloadIdentityCredential   = true,
				ExcludeAzureDeveloperCliCredential  = true,
				ExcludeAzurePowerShellCredential    = true,
				ExcludeSharedTokenCacheCredential   = true,
				ExcludeVisualStudioCodeCredential   = true
			}));
		});

		return services;
	}
}
