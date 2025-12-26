using FluentValidation;
using SharedApi.Authentication.SignUpUser;

namespace Authentication.Presentation.SignUpUser;

internal sealed class SignUpUserRequestValidator : AbstractValidator<SignUpUserRequest>
{
	public SignUpUserRequestValidator()
	{
		RuleFor(r => r.UserName).NotEmpty();
		RuleFor(r => r.Email).NotEmpty();
		RuleFor(r => r.Password).NotEmpty();
		RuleFor(r => r.PasswordRepeat).NotEmpty();
	}
}
