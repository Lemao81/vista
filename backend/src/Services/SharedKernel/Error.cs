namespace SharedKernel;

public abstract record Error
{
	protected Error(string code)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);

		Code = code;
	}

	public string Code { get; }
}
