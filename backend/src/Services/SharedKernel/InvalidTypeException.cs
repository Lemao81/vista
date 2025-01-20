namespace SharedKernel;

public class InvalidTypeException : Exception
{
	public InvalidTypeException()
	{
	}

	public InvalidTypeException(string message) : base(message)
	{
	}

	public InvalidTypeException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
