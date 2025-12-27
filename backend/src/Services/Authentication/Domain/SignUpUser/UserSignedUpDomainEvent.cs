using Common.Domain.Users;
using SharedKernel;

namespace Authentication.Domain.SignUpUser;

public sealed record UserSignedUpDomainEvent(UserId UserId, string UserName, string Email) : IDomainEvent;
