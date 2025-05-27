using Common.Domain.Users;
using SharedKernel;

namespace FileTransfer.Domain.Media;

public sealed class MediaFolder : Entity<MediaFolderId>
{
	private readonly List<MediaItem> _mediaItems = [];

	private MediaFolder(MediaFolderId id, UserId userId, string originalName, byte storageVersion = 0)
	{
		Id             = id;
		UserId         = userId;
		OriginalName   = originalName;
		StorageVersion = storageVersion;
	}

	public override MediaFolderId Id { get; }

	public UserId UserId { get; }

	public byte StorageVersion { get; }

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
