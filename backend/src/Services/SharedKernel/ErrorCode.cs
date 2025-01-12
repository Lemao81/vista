namespace SharedKernel;

public readonly record struct ErrorCode
{
	public ErrorCode() => throw new NotSupportedException();

	public ErrorCode(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

		Value = value;
	}

	public string Value { get; }

	public static implicit operator string(ErrorCode errorCode) => errorCode.Value;
}
