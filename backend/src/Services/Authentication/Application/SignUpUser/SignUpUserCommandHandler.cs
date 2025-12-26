using Authentication.Domain.Constants;
using Authentication.Domain.Exceptions;
using Authentication.Domain.User;
using Common.Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedApi.Authentication.SignUpUser;
using SharedKernel;

namespace Authentication.Application.SignUpUser;

internal sealed class SignUpUserCommandHandler : ICommandHandler<SignUpUserCommand, Result>
{
	private readonly UserManager<AppUser>              _userManager;
	private readonly ILogger<SignUpUserCommandHandler> _logger;

	public SignUpUserCommandHandler(UserManager<AppUser> userManager, ILogger<SignUpUserCommandHandler> logger)
	{
		_userManager = userManager;
		_logger      = logger;
	}

	public async Task<Result> Handle(SignUpUserCommand command, CancellationToken cancellationToken)
	{
		var user = new AppUser
		{
			UserName = command.UserName,
			Email    = command.Email,
		};

		var result = await _userManager.CreateAsync(user, command.Password);
		if (!result.Succeeded)
		{
			_logger.LogError("Creating user failed | Errors={Errors}", result.Errors);

			return new ValidationError(ErrorCodes.ValidationFailed, MapToValidationErrors(result.Errors, command));
		}

		result = await _userManager.AddToRoleAsync(user, UserRoles.Viewer);

		return !result.Succeeded ? throw new AddUserToRoleFailedException(result.Errors) : Result.Success();
	}

	private static Dictionary<string, string[]> MapToValidationErrors(IEnumerable<IdentityError> identityErrors, SignUpUserCommand command)
	{
		var errors = new Dictionary<string, List<string>>(StringComparer.Ordinal);
		foreach (var error in identityErrors)
		{
			var key = error.Code switch
			{
				nameof(IdentityErrorDescriber.DuplicateUserName) or nameof(IdentityErrorDescriber.InvalidUserName) => command.UserNameParameterName,
				nameof(IdentityErrorDescriber.DuplicateEmail) or nameof(IdentityErrorDescriber.InvalidEmail)       => command.EmailParameterName,
				nameof(IdentityErrorDescriber.PasswordRequiresDigit) or nameof(IdentityErrorDescriber.PasswordRequiresLower)
					or nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric) or nameof(IdentityErrorDescriber.PasswordRequiresUpper)
					or nameof(IdentityErrorDescriber.PasswordTooShort) => command.PasswordParameterName,
				nameof(IdentityErrorDescriber.PasswordMismatch) => command.PasswordRepeatParameterName,
				_                                               => command.RootParameterName,
			};

			if (!errors.TryGetValue(key, out var value))
			{
				value = [];
				errors.Add(key, value);
			}

			value.Add(error.Description);
		}

		return errors.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray(), StringComparer.Ordinal);
	}
}
