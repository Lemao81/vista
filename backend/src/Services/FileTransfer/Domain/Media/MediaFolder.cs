using Common.Domain.Users;
using SharedKernel;

namespace FileTransfer.Domain.Media;

public sealed class MediaFolder : Entity<MediaFolderId>
{
	private readonly List<MediaItem> _mediaItems = [];

	private MediaFolder(MediaFolderId id, UserId userId, string originalName, byte storageVersion)
	{
		Id             = id;
		UserId         = userId;
		OriginalName   = originalName;
		StorageVersion = storageVersion;
	}

	public override MediaFolderId Id { get; }

	public UserId UserId { get; private set; }

	public byte StorageVersion { get; private set; }

	public string OriginalName { get; private set; }

	public IReadOnlyCollection<MediaItem> MediaItems => _mediaItems;

	public static MediaFolder Create(UserId userId, string originalName) => new(new MediaFolderId(Guid.NewGuid()), userId, originalName, 0);

	public void AddMediaItem(MediaItem mediaItem)
	{
		if (mediaItem.UserId != UserId)
		{
			throw new ArgumentException($"User ids don't match: Folder: {UserId}, Item: {mediaItem.UserId}", paramName: nameof(mediaItem));
		}

		_mediaItems.Add(mediaItem);
	}
}
