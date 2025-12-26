using FluentValidation;
using SharedApi.Authentication.SignUpUser;

namespace Authentication.Application.SignUpUser;

public class SignUpUserCommandValidator : AbstractValidator<SignUpUserCommand>
{
	public SignUpUserCommandValidator()
	{
		RuleFor(c => c.Email).EmailAddress();
		RuleFor(c => c.PasswordRepeat)
			.Must((c, passwordRepeat) => string.Equals(passwordRepeat, c.Password, StringComparison.Ordinal))
			.WithMessage("Password repeat must equal password");
	}
}
