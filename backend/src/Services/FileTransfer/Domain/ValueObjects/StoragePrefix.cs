namespace FileTransfer.Domain.ValueObjects;

public readonly record struct StoragePrefix
{
	public StoragePrefix() => throw new NotSupportedException();

	public StoragePrefix(params object[] parts)
	{
		Value = parts.Length == 0
			        ? string.Empty
			        : $"/{parts.Select(p => (p.ToString() ?? string.Empty).Trim('/').ToLowerInvariant()).Aggregate((a, b) => $"{a}/{b}")}";
	}

	public string Value { get; }
}
