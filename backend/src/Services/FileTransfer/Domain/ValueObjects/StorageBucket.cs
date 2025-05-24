using Common.Domain.Constants.Storage;

namespace FileTransfer.Domain.ValueObjects;

public readonly record struct StorageBucket
{
	public static readonly StorageBucket Media = new(Buckets.Media);

	public StorageBucket() => throw new NotSupportedException();

	private StorageBucket(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

		Value = value;
	}

	public string Value { get; }
}
