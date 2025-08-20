using Common.Application.Abstractions;
using Common.Domain.Users;
using FileTransfer.Application.Utilities;
using FileTransfer.Domain.Media;
using FileTransfer.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SharedKernel;

namespace FileTransfer.Application.Pictures.Upload;

internal sealed class UploadPictureCommandHandler : ICommandHandler<UploadPictureCommand, Result<UploadPictureResponse>>
{
	private readonly IMediaFolderRepository               _mediaFolderRepository;
	private readonly IObjectStorageAdapter                _objectStorageAdapter;
	private readonly ILogger<UploadPictureCommandHandler> _logger;

	public UploadPictureCommandHandler(
		IMediaFolderRepository               mediaFolderRepository,
		IObjectStorageAdapter                objectStorageAdapter,
		ILogger<UploadPictureCommandHandler> logger)
	{
		_mediaFolderRepository = mediaFolderRepository;
		_objectStorageAdapter  = objectStorageAdapter;
		_logger                = logger;
	}

	public async Task<Result<UploadPictureResponse>> Handle(UploadPictureCommand command, CancellationToken cancellationToken)
	{
		try
		{
			var userId      = new UserId(Guid.Parse("f6fac46a-ad79-44d8-8bc5-917e8cbad737"));
			var mediaFolder = MediaFolder.Create(userId, command.FileName.Value);
			var mediaItem   = MediaItem.Create(mediaFolder.Id, userId, MediaKind.Picture, MediaSizeKind.Original, command.MediaType);
			mediaFolder.AddMediaItem(mediaItem);

			var fileName   = ApplicationHelper.GetStorageFileName(command.FileName, mediaItem);
			var objectName = StorageObjectName.CreateMediaName(fileName, userId, mediaFolder.Id);
			var putObjectResult = await _objectStorageAdapter.PutObjectAsync(
				                      StorageBucket.Media,
				                      objectName,
				                      command.MediaType,
				                      command.Stream,
				                      cancellationToken);

			if (putObjectResult.IsFailure)
			{
				return putObjectResult.Error;
			}

			mediaFolder = await _mediaFolderRepository.AddAsync(mediaFolder);

			using (LogContext.PushProperty("MediaFolder", mediaFolder, destructureObjects: true))
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
