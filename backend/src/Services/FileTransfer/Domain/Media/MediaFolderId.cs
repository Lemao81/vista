﻿using Lemao.UtilExtensions;

namespace FileTransfer.Domain.Media;

public record MediaFolderId
{
	public MediaFolderId(Guid value)
	{
		if (value.IsEmpty())
		{
			throw new ArgumentException($"'{nameof(value)}' cannot be empty", nameof(value));
		}

		Value = value;
	}

	public Guid Value { get; }

	public static implicit operator Guid(MediaFolderId folderId) => folderId.Value;
}
