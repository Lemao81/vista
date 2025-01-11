using Domain.Media;
using Domain.Users;

namespace Domain.ValueObjects;

public readonly record struct StorageObjectName
{
	public StorageObjectName(FileName fileName, params object[] parts)
	{
		Value = "/" + fileName.Value;
		if (parts.Length != 0)
		{
			Value = $"/{parts.Select(p => (p.ToString() ?? "").Trim('/').ToLowerInvariant()).Aggregate((a, b) => $"{a}/{b}")}{Value}";
		}
	}

	public string Value { get; }

	public static StorageObjectName CreateMediaName(FileName fileName, UserId userId, MediaFolderId mediaFolderId) =>
		new(fileName, userId.Value, mediaFolderId.Value);
}
