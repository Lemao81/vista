using Common.Domain.Users;
using FileTransfer.Domain.Media;
using SharedKernel;

namespace FileTransfer.Domain.ValueObjects;

public record StorageObjectName
{
	public StorageObjectName(FileName fileName, params object[] parts)
	{
		Value = "/" + fileName.NormalizedValue;
		if (parts.Length != 0)
		{
			Value = $"/{parts.Select(p => (p.ToString() ?? string.Empty).Trim('/').ToLowerInvariant()).Aggregate((a, b) => $"{a}/{b}")}{Value}";
		}
	}

	public string Value { get; }

	public static StorageObjectName CreateMediaName(FileName fileName, UserId userId, MediaFolderId mediaFolderId) =>
		new(fileName, userId.Value, mediaFolderId.Value);
}
