namespace SharedKernel;

public readonly record struct FileLength : IComparable, IComparable<FileLength>
{
	public FileLength() => throw new NotSupportedException();

	public FileLength(long value)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value);

		Value = (ulong)value;
	}

	public ulong Value { get; }

	public int CompareTo(object? obj) => Value.CompareTo(obj);

	public int CompareTo(FileLength other) => Value.CompareTo(other.Value);

	public static implicit operator FileLength(long value)      => new(value);
	public static implicit operator long(FileLength fileLength) => (long)fileLength.Value;
}
