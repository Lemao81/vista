namespace SharedKernel;

public readonly record struct FileName
{
	public FileName() => throw new NotSupportedException();

	public FileName(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value);

		var dotIndex = value.LastIndexOf('.');
		if (dotIndex < 0 || dotIndex == value.Length - 1)
		{
			throw new ArgumentException($"'{nameof(value)}' must have an extension", nameof(value));
		}

		Value           = value;
		BaseName        = value[..dotIndex];
		Extension       = value[(dotIndex + 1)..].ToLowerInvariant();
		NormalizedValue = value.ToLowerInvariant();
	}

	public string Value { get; }

	public string BaseName { get; }

	public string Extension { get; }

	public string NormalizedValue { get; }

	public static implicit operator FileName(string value) => new(value);

	public static implicit operator string(FileName fileName) => fileName.Value;
}
