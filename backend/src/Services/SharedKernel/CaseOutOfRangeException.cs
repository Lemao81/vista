namespace SharedKernel;

public class CaseOutOfRangeException : Exception
{
	public CaseOutOfRangeException()
	{
	}

	public CaseOutOfRangeException(string message) : base(message)
	{
	}

	public CaseOutOfRangeException(string paramName, object actualValue) : base($"Case '{actualValue}' is out of range of {paramName}")
	{
	}

	public CaseOutOfRangeException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
