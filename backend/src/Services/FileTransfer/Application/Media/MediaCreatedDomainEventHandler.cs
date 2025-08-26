using FileTransfer.Domain.Media;
using MediatR;
using SharedKernel;

namespace FileTransfer.Application.Media;

internal sealed class MediaCreatedDomainEventHandler : INotificationHandler<MediaCreatedDomainEvent>
{
	private readonly FileTransferMetrics _fileTransferMetrics;

	public MediaCreatedDomainEventHandler(FileTransferMetrics fileTransferMetrics)
	{
		_fileTransferMetrics = fileTransferMetrics;
	}

	public Task Handle(MediaCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		switch (domainEvent.MediaKind)
		{
			case MediaKind.Image:
				_fileTransferMetrics.ImageUploaded(domainEvent.MediaType);

				break;
			case MediaKind.Video:
				break;
			default:
				throw new CaseOutOfRangeException(nameof(domainEvent.MediaKind), domainEvent.MediaKind);
		}

		return Task.CompletedTask;
	}
}
