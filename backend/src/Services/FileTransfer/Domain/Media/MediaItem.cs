using Domain.Abstractions;
using Domain.Users;

namespace Domain.Media;

public sealed class MediaItem : Entity<MediaItemId>
{
	private MediaItem(MediaItemId id, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind)
	{
		Id = id;
		UserId = userId;
		MediaKind = mediaKind;
		MediaSizeKind = mediaSizeKind;
	}

	public override MediaItemId Id { get; protected set; }
	public UserId UserId { get; private set; }
	public MediaKind MediaKind { get; private set; }
	public MediaSizeKind MediaSizeKind { get; private set; }

	public static MediaItem Create(UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind) => new(Guid.NewGuid(), userId, mediaKind, mediaSizeKind);
}
