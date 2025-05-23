﻿using Common.Application.Constants;
using Lemao.UtilExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using SharedKernel;

namespace Common.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMinio(client =>
		{
			var endpoint = configuration[ConfigurationKeys.MinioEndpoint];
			if (endpoint.IsNullOrWhiteSpace())
			{
				throw new MissingConfigurationException("Minio endpoint");
			}

			var accessKey = configuration[ConfigurationKeys.MinioAccessKey];
			var secretKey = configuration[ConfigurationKeys.MinioSecretKey];
			if (accessKey.IsNullOrWhiteSpace())
			{
				var keysFile = configuration[ConfigurationKeys.MinioKeysFile];
				if (!keysFile.IsNullOrWhiteSpace() && File.Exists(keysFile))
				{
					var keys     = File.ReadAllText(keysFile);
					var keySplit = keys.Split(':');
					accessKey = keySplit[0];
					secretKey = keySplit[1];
				}
			}

			if (accessKey.IsNullOrWhiteSpace())
			{
				throw new MissingConfigurationException("Minio access key");
			}

			if (secretKey.IsNullOrWhiteSpace())
			{
				throw new MissingConfigurationException("Minio secret key");
			}

			client.WithEndpoint(endpoint).WithCredentials(accessKey, secretKey).WithSSL(false);
		});

		return services;
	}
}
