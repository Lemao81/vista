using Application.Abstractions;
using Domain;
using Domain.Media;
using Domain.Users;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application.Pictures.Upload;

internal sealed class UploadPictureCommandHandler : ICommandHandler<UploadPictureCommand, Result<UploadPictureResponse>>
{
	private readonly IMediaFolderRepository               _mediaFolderRepository;
	private readonly ILogger<UploadPictureCommandHandler> _logger;

	public UploadPictureCommandHandler(IMediaFolderRepository mediaFolderRepository, ILogger<UploadPictureCommandHandler> logger)
	{
		_mediaFolderRepository = mediaFolderRepository;
		_logger                = logger;
	}

	public async Task<Result<UploadPictureResponse>> Handle(UploadPictureCommand command, CancellationToken cancellationToken)
	{
		try
		{
			var userId      = new UserId(Guid.NewGuid());
			var mediaFolder = MediaFolder.Create(userId);
			var mediaItem   = MediaItem.Create(mediaFolder.Id, userId, MediaKind.Picture, MediaSizeKind.Original);
			mediaFolder.AddMediaItem(mediaItem);
			using var ms = new MemoryStream();
			await command.Stream.CopyToAsync(ms, cancellationToken);
			mediaFolder = await _mediaFolderRepository.AddMediaFolderAsync(mediaFolder, ms.ToArray());

			using (LogContext.PushProperty("MediaFolder", mediaFolder, true))
			{
				_logger.LogInformation("Picture uploaded successfully");
			}

			return new UploadPictureResponse(mediaFolder.Id);
		}
		finally
		{
			await command.Stream.DisposeAsync();
		}
	}
}
