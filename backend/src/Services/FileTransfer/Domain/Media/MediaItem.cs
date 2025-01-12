using Domain.Users;
using SharedKernel;

namespace Domain.Media;

public sealed class MediaItem : Entity<MediaItemId>
{
	private Dictionary<string, object> _metadata = [];

	private MediaItem(MediaItemId id, MediaFolderId mediaFolderId, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind)
	{
		Id            = id;
		MediaFolderId = mediaFolderId;
		UserId        = userId;
		MediaKind     = mediaKind;
		MediaSizeKind = mediaSizeKind;
	}

	public override MediaItemId   Id             { get; protected set; }
	public          MediaFolderId MediaFolderId  { get; private set; }
	public          UserId        UserId         { get; private set; }
	public          MediaKind     MediaKind      { get; private set; }
	public          MediaSizeKind MediaSizeKind  { get; private set; }
	public          byte          StorageVersion { get; private set; }

	public IReadOnlyDictionary<string, object> MetaData
	{
		get => _metadata;
		private set => _metadata = new Dictionary<string, object>(value);
	}

	public void AddMetaData(string key, object value) => _metadata[key] = value;

	public static MediaItem Create(MediaFolderId mediaFolderId, UserId userId, MediaKind mediaKind, MediaSizeKind mediaSizeKind)
	{
		var item = new MediaItem(new MediaItemId(Guid.NewGuid()), mediaFolderId, userId, mediaKind, mediaSizeKind);
		item.RaiseDomainEvent(new MediaCreatedDomainEvent(item.MediaFolderId, item.Id, mediaKind));

		return item;
	}
}
