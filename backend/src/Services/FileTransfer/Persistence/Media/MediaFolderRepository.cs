using Domain.Media;

namespace Persistence.Media;

internal sealed class MediaFolderRepository : IMediaFolderRepository
{
	private readonly IObjectStorage _objectStorage;

	public MediaFolderRepository(IObjectStorage objectStorage)
	{
		_objectStorage = objectStorage;
	}

	public Task<MediaFolder> AddMediaFolderAsync(MediaFolder mediaFolder)
	{
		throw new NotImplementedException();
	}
}
