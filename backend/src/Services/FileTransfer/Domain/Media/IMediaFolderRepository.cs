using Domain.Abstractions;

namespace Domain.Media;

public interface IMediaFolderRepository : IRepository
{
	Task<MediaFolder> AddMediaFolderAsync(MediaFolder mediaFolder, byte[] bytes);
}
