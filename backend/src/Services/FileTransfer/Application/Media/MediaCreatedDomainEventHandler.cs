using Domain.Media;
using MediatR;

namespace Application.Media;

internal sealed class MediaCreatedDomainEventHandler : INotificationHandler<MediaCreatedDomainEvent>
{
	public Task Handle(MediaCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		Console.WriteLine("A media has been created");

		return Task.CompletedTask;
	}
}
