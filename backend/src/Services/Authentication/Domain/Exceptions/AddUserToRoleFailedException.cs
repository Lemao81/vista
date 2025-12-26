using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Exceptions;

public class AddUserToRoleFailedException : Exception
{
	public AddUserToRoleFailedException()
	{
	}

	public AddUserToRoleFailedException(IEnumerable<IdentityError> identityErrors) : base($"Adding user to role failed | Errors={identityErrors}")
	{
	}

	public AddUserToRoleFailedException(string message) : base(message)
	{
	}

	public AddUserToRoleFailedException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
