using Domain.Abstractions;
using Domain.Users;

namespace Domain.Media;

public sealed class MediaItem : Entity<MediaItemId>
{
	private MediaItem(MediaItemId id, MediaFolderId mediaFolderId, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind)
	{
		Id            = id;
		MediaFolderId = mediaFolderId;
		UserId        = userId;
		MediaKind     = mediaKind;
		MediaSizeKind = mediaSizeKind;
	}

	public override MediaItemId   Id            { get; protected set; }
	public          MediaFolderId MediaFolderId { get; private set; }
	public          UserId        UserId        { get; private set; }
	public          MediaKind     MediaKind     { get; private set; }
	public          MediaSizeKind MediaSizeKind { get; private set; }

	public static MediaItem Create(MediaFolderId mediaFolderId, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind) =>
		new(new MediaItemId(Guid.NewGuid()), mediaFolderId, userId, mediaKind, mediaSizeKind);
}
