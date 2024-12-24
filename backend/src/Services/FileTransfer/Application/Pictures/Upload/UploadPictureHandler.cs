using Domain.Media;
using MediatR;

namespace Application.Pictures.Upload;

internal sealed class UploadPictureHandler : IRequestHandler<UploadPictureCommand, UploadPictureResponse>
{
	private readonly IMediaFolderRepository _mediaFolderRepository;

	public UploadPictureHandler(IMediaFolderRepository mediaFolderRepository)
	{
		_mediaFolderRepository = mediaFolderRepository;
	}

	public async Task<UploadPictureResponse> Handle(UploadPictureCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var userId      = Guid.NewGuid();
			var mediaFolder = MediaFolder.Create(userId);
			var mediaItem   = MediaItem.Create(userId, MediaKind.Picture, MediaSizeKind.Original);
			mediaFolder.AddMediaItem(mediaItem);
			mediaFolder = await _mediaFolderRepository.AddMediaFolderAsync(mediaFolder);

			return new UploadPictureResponse(mediaFolder.Id);
		}
		finally
		{
			await request.Stream.DisposeAsync();
		}
	}
}
