using Authentication.Domain.SignUpUser;
using Common.Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Authentication.Domain.User;

public sealed class AppUser : IdentityUser<Guid>, IEntity
{
	private readonly List<IDomainEvent> _domainEvents = [];

	public DateTime CreatedUtc { get; set; } = default;

	public DateTime? ModifiedUtc { get; set; }

	public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

	public bool HasDomainEvents => _domainEvents.Count > 0;

	public static AppUser Create(string userName, string email)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(userName);
		ArgumentException.ThrowIfNullOrWhiteSpace(email);

		var user = new AppUser
		{
			Id       = Guid.NewGuid(),
			UserName = userName,
			Email    = email,
		};

		user.AddDomainEvent(new UserSignedUpDomainEvent(new UserId(user.Id), user.UserName, user.Email));

		return user;
	}

	public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

	public void ClearDomainEvents() => _domainEvents.Clear();
}
