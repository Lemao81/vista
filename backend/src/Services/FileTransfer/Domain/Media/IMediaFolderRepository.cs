using SharedKernel;

namespace Domain.Media;

public interface IMediaFolderRepository : IRepository
{
	Task<MediaFolder> AddAsync(MediaFolder mediaFolder);
}
