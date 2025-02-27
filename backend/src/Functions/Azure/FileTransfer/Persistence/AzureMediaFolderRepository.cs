using Domain.Media;

namespace FileTransfer.Persistence;

public class AzureMediaFolderRepository : IMediaFolderRepository
{
	public Task<MediaFolder> AddAsync(MediaFolder mediaFolder)
	{
		throw new NotImplementedException();
	}
}
