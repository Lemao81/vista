namespace Domain.ValueObjects;

public readonly record struct FileLength
{
	public FileLength(long value)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value);

		Value = (ulong)value;
	}

	public ulong Value { get; }

	public static implicit operator FileLength(long value)      => new(value);
	public static implicit operator long(FileLength fileLength) => (long)fileLength.Value;
}
