using UtilExtensions;

namespace Domain.Users;

public record UserId
{
	public UserId(Guid value)
	{
		if (value.IsEmpty())
		{
			throw new ArgumentException($"'{nameof(value)}' cannot be empty.", nameof(value));
		}

		Value = value;
	}

	public Guid Value { get; }

	public static implicit operator Guid(UserId folderId) => folderId.Value;
	public static implicit operator UserId(Guid id)       => new(id);
}
