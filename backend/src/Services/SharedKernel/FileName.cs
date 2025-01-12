namespace SharedKernel;

public readonly record struct FileName
{
	public FileName() => throw new NotSupportedException();

	public FileName(string value)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
		
		var dotIndex = value.LastIndexOf('.');
		if (dotIndex < 0 || dotIndex == value.Length - 1)
		{
			throw new ArgumentException($"'{nameof(value)}' must have an extension", nameof(value));
		}

		Value     = value.ToLowerInvariant();
		Name      = value[..dotIndex].ToLowerInvariant();
		Extension = value[(dotIndex + 1)..].ToLowerInvariant();
	}

	public string Value     { get; }
	public string Name      { get; }
	public string Extension { get; }

	public static implicit operator FileName(string value)    => new(value);
	public static implicit operator string(FileName fileName) => fileName.Value;
}
