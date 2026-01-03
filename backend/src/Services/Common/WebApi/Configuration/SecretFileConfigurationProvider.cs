using Common.Application.Constants;
using Lemao.UtilExtensions;
using Microsoft.Extensions.Configuration;

namespace Common.WebApi.Configuration;

public class SecretFileConfigurationProvider : ConfigurationProvider
{
	private readonly string? _databasePasswordFilePath;
	private readonly string? _jwtSecretKeyFilePath;

	public SecretFileConfigurationProvider(string? databasePasswordFilePath, string? jwtSecretKeyFilePath)
	{
		_databasePasswordFilePath = databasePasswordFilePath;
		_jwtSecretKeyFilePath     = jwtSecretKeyFilePath;
	}

	public override void Load()
	{
		base.Load();

		if (!_databasePasswordFilePath.IsNullOrWhiteSpace() && File.Exists(_databasePasswordFilePath))
		{
			var dbPassword = File.ReadAllText(_databasePasswordFilePath);
			Data.Add(ConfigurationKeys.DatabasePassword, dbPassword);
		}

		if (!_jwtSecretKeyFilePath.IsNullOrWhiteSpace() && File.Exists(_jwtSecretKeyFilePath))
		{
			var jwtSecretKey = File.ReadAllText(_jwtSecretKeyFilePath);
			Data.Add(ConfigurationKeys.JwtSecretKey, jwtSecretKey);
		}
	}
}
