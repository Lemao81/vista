using Common.Application.Constants;
using Common.WebApi.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Common.WebApi.Extensions;

public static class ConfigurationExtensions
{
	public static ConfigurationManager AddCommonAppSettings(this ConfigurationManager builder)
	{
		builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings-common.json"), optional: false, reloadOnChange: true);
		var sources = builder.Sources;
		var index   = sources.IndexOf(sources.First(s => s is JsonConfigurationSource));
		var last    = sources[^1];
		sources.RemoveAt(sources.Count - 1);
		sources.Insert(index, last);

		return builder;
	}

	public static ConfigurationManager AddSecretFileConfiguration(this ConfigurationManager builder)
	{
		((IConfigurationBuilder)builder).Add(
			new SecretFileConfigurationSource(
				databasePasswordFilePath: builder[ConfigurationKeys.DatabasePasswordFile],
				jwtSecretKeyFilePath: builder[ConfigurationKeys.JwtSecretKeyFile]));

		return builder;
	}
}
