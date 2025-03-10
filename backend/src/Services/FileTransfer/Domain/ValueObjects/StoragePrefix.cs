namespace FileTransfer.Domain.ValueObjects;

public readonly record struct StoragePrefix
{
	public StoragePrefix() => throw new NotSupportedException();

	public StoragePrefix(params object[] parts)
	{
		Value = parts.Length == 0 ? "" : $"/{parts.Select(p => (p.ToString() ?? "").Trim('/').ToLowerInvariant()).Aggregate((a, b) => $"{a}/{b}")}";
	}

	public string Value { get; }
}
