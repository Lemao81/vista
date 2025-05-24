using Common.Domain.Users;
using SharedKernel;

namespace FileTransfer.Domain.Media;

public sealed class MediaFolder : Entity<MediaFolderId>
{
	private readonly List<MediaItem> _mediaItems = [];

	private MediaFolder(MediaFolderId id, UserId userId, string originalName)
	{
		Id           = id;
		UserId       = userId;
		OriginalName = originalName;
	}

	public override MediaFolderId Id { get; protected set; }

	public UserId UserId { get; private set; }

	public byte StorageVersion { get; private set; }

	public string OriginalName { get; private set; }

	public IReadOnlyCollection<MediaItem> MediaItems => _mediaItems;

	public static MediaFolder Create(UserId userId, string originalName) => new(new MediaFolderId(Guid.NewGuid()), userId, originalName);

	public void AddMediaItem(MediaItem mediaItem)
	{
		if (mediaItem.UserId != UserId)
		{
			throw new ArgumentException($"User {UserId} does not have the same user id {mediaItem.UserId}");
		}

		_mediaItems.Add(mediaItem);
	}
}
