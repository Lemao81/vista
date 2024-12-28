namespace Domain.Media;

public interface IMediaFolderRepository
{
	Task<MediaFolder> AddMediaFolderAsync(MediaFolder mediaFolder, byte[] bytes);
}
