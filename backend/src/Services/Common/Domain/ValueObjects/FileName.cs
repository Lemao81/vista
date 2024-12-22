namespace Domain.ValueObjects;

public readonly record struct FileName
{
	public FileName(string value)
	{
		var dotIndex = value.LastIndexOf('.');
		if (dotIndex < 0 || dotIndex == value.Length - 1)
		{
			throw new ArgumentException($"File name must have an extension - given '{value}'");
		}

		Value = value;
		Name = value[..dotIndex];
		Extension = value[(dotIndex + 1)..];
	}

	public string Value { get; }
	public string Name { get; }
	public string Extension { get; }

	public static implicit operator FileName(string value) => new(value);
	public static implicit operator string(FileName fileName) => fileName.Value;
}
