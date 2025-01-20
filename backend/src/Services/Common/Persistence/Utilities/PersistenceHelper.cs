using Application;
using Lemao.UtilExtensions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SharedKernel;

namespace Persistence.Utilities;

public static class PersistenceHelper
{
	public static NpgsqlDataSource CreateDataSource(IConfiguration configuration, string database, bool persistSecurityInfo = false)
	{
		var host = configuration[ConfigurationKeys.DatabaseHost];
		if (host.IsNullOrWhiteSpace())
		{
			host = Constants.DefaultDatabaseHost;
		}

		var username = configuration[ConfigurationKeys.DatabaseUsername];
		if (username.IsNullOrWhiteSpace())
		{
			throw new MissingConfigurationException("Database username is not configured");
		}

		var password = configuration[ConfigurationKeys.DatabasePassword];
		if (password.IsNullOrWhiteSpace())
		{
			var passwordFile = configuration[ConfigurationKeys.DatabasePasswordFile];
			if (!passwordFile.IsNullOrWhiteSpace() && File.Exists(passwordFile))
			{
				password = File.ReadAllText(passwordFile);
			}
		}

		if (password.IsNullOrWhiteSpace())
		{
			throw new MissingConfigurationException("Database password is not configured");
		}

		var builder = new NpgsqlDataSourceBuilder
		{
			ConnectionStringBuilder =
			{
				Host                = host,
				Database            = database,
				Username            = username,
				Password            = password,
				PersistSecurityInfo = persistSecurityInfo
			}
		};

		return builder.Build();
	}
}
