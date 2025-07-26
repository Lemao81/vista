﻿#if TEST
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

#if TEST
			Console.WriteLine("TEST compiler constant set");
#endif

			Console.WriteLine("CONSOLEWRITETESTCONSOLEWRITETESTCONSOLEWRITETESTCONSOLEWRITETESTCONSOLEWRITETESTCONSOLEWRITETEST");
#if RELEASE
			Console.WriteLine("RELEASERELEASERELEASERELEASERELEASERELEASERELEASERELEASERELEASERELEASERELEASERELEASERELEASE");
#endif
#if TEST
			Console.WriteLine("TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST");
#endif
			var managedIdentityClientId = configuration.GetValue<string>(ConfigurationKeys.AzureStorageManagedIdentityClientId);
			clientBuilder.UseCredential(
				new DefaultAzureCredential(
					new DefaultAzureCredentialOptions
					{
						ManagedIdentityClientId             = managedIdentityClientId,
						ExcludeManagedIdentityCredential    = true,
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
