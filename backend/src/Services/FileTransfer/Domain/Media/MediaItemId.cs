﻿using UtilExtensions;

namespace Domain.Media;

public record MediaItemId
{
	public MediaItemId(Guid value)
	{
		if (value.IsEmpty())
		{
			throw new ArgumentException($"'{nameof(value)}' cannot be empty.", nameof(value));
		}

		Value = value;
	}

	public Guid Value { get; }

	public static implicit operator Guid(MediaItemId folderId) => folderId.Value;
	public static implicit operator MediaItemId(Guid id) => new(id);
}
