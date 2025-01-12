namespace Domain.ValueObjects;

public readonly record struct StorageBucket
{
	public StorageBucket() => throw new NotSupportedException();

	private StorageBucket(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

		Value = value;
	}

	public string Value { get; }

	public static readonly StorageBucket Media = new(Storage.Buckets.Media);
}
