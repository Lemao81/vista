using Microsoft.Extensions.Configuration;

namespace Common.WebApi.Configuration;

public class SecretFileConfigurationSource : IConfigurationSource
{
	private readonly string? _databasePasswordFilePath;
	private readonly string? _jwtSecretKeyFilePath;

	public SecretFileConfigurationSource(string? databasePasswordFilePath, string? jwtSecretKeyFilePath)
	{
		_databasePasswordFilePath = databasePasswordFilePath;
		_jwtSecretKeyFilePath     = jwtSecretKeyFilePath;
	}

	public IConfigurationProvider Build(IConfigurationBuilder builder) => new SecretFileConfigurationProvider(_databasePasswordFilePath, _jwtSecretKeyFilePath);
}
