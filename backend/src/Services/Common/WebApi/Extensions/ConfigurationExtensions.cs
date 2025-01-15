using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace WebApi.Extensions;

public static class ConfigurationExtensions
{
	public static ConfigurationManager AddCommonAppSettings(this ConfigurationManager builder)
	{
		builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings-common.json"), false, true);
		var sources = builder.Sources;
		var index   = sources.IndexOf(sources.First(s => s is JsonConfigurationSource));
		var last    = sources[^1];
		sources.RemoveAt(sources.Count - 1);
		sources.Insert(index, last);

		return builder;
	}
}
