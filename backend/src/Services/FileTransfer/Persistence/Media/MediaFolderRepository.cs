using Domain.Media;

namespace Persistence.Media;

internal sealed class MediaFolderRepository : IMediaFolderRepository
{
	private readonly FileTransferDbContext _dbContext;

	public MediaFolderRepository(FileTransferDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<MediaFolder> AddAsync(MediaFolder mediaFolder)
	{
		var entry = await _dbContext.MediaFolders.AddAsync(mediaFolder);

		return entry.Entity;
	}
}
