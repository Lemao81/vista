using Domain.Media;

namespace Persistence.Media;

internal sealed class MediaFolderRepository : IMediaFolderRepository
{
	private readonly FileTransferDbContext _dbContext;
	private readonly IObjectStorage        _objectStorage;

	public MediaFolderRepository(FileTransferDbContext dbContext, IObjectStorage objectStorage)
	{
		_dbContext     = dbContext;
		_objectStorage = objectStorage;
	}

	public async Task<MediaFolder> AddMediaFolderAsync(MediaFolder mediaFolder, byte[] bytes)
	{
		var entry = await _dbContext.MediaFolders.AddAsync(mediaFolder);

		return entry.Entity;
	}
}
