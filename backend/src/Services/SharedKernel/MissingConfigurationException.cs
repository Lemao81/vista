namespace SharedKernel;

public class MissingConfigurationException : Exception
{
	public MissingConfigurationException()
	{
	}

	public MissingConfigurationException(string name) : base($"{name} is not configured")
	{
	}

	public MissingConfigurationException(string name, Exception innerException) : base($"{name} is not configured", innerException)
	{
	}
}
