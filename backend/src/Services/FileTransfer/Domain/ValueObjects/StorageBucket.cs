using Lemao.UtilExtensions;

namespace Domain.ValueObjects;

public readonly record struct StorageBucket
{
	private StorageBucket(string value)
	{
		if (value.IsNullOrWhiteSpace())
		{
			throw new ArgumentException($"'{nameof(value)}' must not be empty");
		}

		Value = value;
	}

	public string Value { get; }

	public static readonly StorageBucket Media = new(Storage.Buckets.Media);
}
