using Common.Domain.Users;
using SharedKernel;

namespace FileTransfer.Domain.Media;

public sealed class MediaItem : Entity<MediaItemId>
{
	private Dictionary<string, object> _metadata = [];

	private MediaItem(MediaItemId id, MediaFolderId mediaFolderId, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind, byte storageVersion)
	{
		Id             = id;
		MediaFolderId  = mediaFolderId;
		UserId         = userId;
		MediaKind      = mediaKind;
		MediaSizeKind  = mediaSizeKind;
		StorageVersion = storageVersion;
	}

	public override MediaItemId Id { get; }

	public MediaFolderId MediaFolderId { get; private set; }

	public UserId UserId { get; private set; }

	public MediaKind MediaKind { get; private set; }

	public MediaSizeKind MediaSizeKind { get; private set; }

	public byte StorageVersion { get; private set; }

	public IReadOnlyDictionary<string, object> MetaData
	{
		get => _metadata;
#pragma warning disable S1144
		private set => _metadata = new Dictionary<string, object>(value);
#pragma warning restore S1144
	}

	public static MediaItem Create(MediaFolderId mediaFolderId, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind, string mediaType)
	{
		var item = new MediaItem(new MediaItemId(Guid.NewGuid()), mediaFolderId, userId, mediaKind, mediaSizeKind, 0);
		item.AddDomainEvent(new MediaCreatedDomainEvent(item.MediaFolderId, item.Id, mediaKind, mediaType));

		return item;
	}

	public void AddMetaData(string key, object value) => _metadata[key] = value;
}
