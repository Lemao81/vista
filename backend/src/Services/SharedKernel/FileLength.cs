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

	public static implicit operator FileLength(long value) => new(value);

	public static implicit operator long(FileLength fileLength) => (long)fileLength.Value;

	public static bool operator <(FileLength left, FileLength right) => left.CompareTo(right) < 0;

	public static bool operator <=(FileLength left, FileLength right) => left.CompareTo(right) <= 0;

	public static bool operator >(FileLength left, FileLength right) => left.CompareTo(right) > 0;

	public static bool operator >=(FileLength left, FileLength right) => left.CompareTo(right) >= 0;

	public int CompareTo(object? obj) => Value.CompareTo(obj);

	public int CompareTo(FileLength other) => Value.CompareTo(other.Value);
}
