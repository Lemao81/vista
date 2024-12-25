using Domain.Abstractions;
using Domain.Users;

namespace Domain.Media;

public sealed class MediaFolder : Entity<MediaFolderId>
{
	private readonly List<MediaItem> _mediaItems = [];

	private MediaFolder(MediaFolderId id, UserId userId)
	{
		Id     = id;
		UserId = userId;
	}

	public override MediaFolderId                  Id         { get; protected set; }
	public          UserId                         UserId     { get; private set; }
	public          IReadOnlyCollection<MediaItem> MediaItems => _mediaItems;

	public void AddMediaItem(MediaItem mediaItem)
	{
		if (mediaItem.UserId != UserId)
		{
			throw new ArgumentException($"User {UserId} does not have the same user id {mediaItem.UserId}");
		}

		_mediaItems.Add(mediaItem);
	}

	public static MediaFolder Create(UserId userId) => new(new MediaFolderId(Guid.NewGuid()), userId);
}
