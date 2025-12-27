using Authentication.Domain.SignUpUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.SignUpUser;

internal sealed class UserSignedUpDomainEventHandler : INotificationHandler<UserSignedUpDomainEvent>
{
	private readonly ILogger<UserSignedUpDomainEventHandler> _logger;

	public UserSignedUpDomainEventHandler(ILogger<UserSignedUpDomainEventHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(UserSignedUpDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		_logger.LogInformation("New user signed up | UserId='{UserId}' UserName='{UserName}'", domainEvent.UserId, domainEvent.UserName);

		return Task.CompletedTask;
	}
}
