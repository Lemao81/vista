using Common.Domain.Constants.Storage;

namespace FileTransfer.Domain.ValueObjects;

public record StorageBucket
{
	public static readonly StorageBucket Media = new(Buckets.Media);

	private StorageBucket(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);

		Value = value;
	}

	public string Value { get; }
}
