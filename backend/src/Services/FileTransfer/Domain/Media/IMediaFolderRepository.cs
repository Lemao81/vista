using SharedKernel;

namespace FileTransfer.Domain.Media;

public interface IMediaFolderRepository : IRepository
{
	Task<MediaFolder> AddAsync(MediaFolder mediaFolder);
}
