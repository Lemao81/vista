using SharedApi.Authentication.SignUpUser;

namespace SharedApi.Authentication;

public static class CommandFactory
{
	public static SignUpUserCommand CreateSignUpUserCommand(SignUpUserRequest request) =>
		new(
			request.UserName,
			request.Email,
			request.Password,
			request.PasswordRepeat,
			nameof(SignUpUserRequest.UserName),
			nameof(SignUpUserRequest.Email),
			nameof(SignUpUserRequest.Password),
			nameof(SignUpUserRequest.PasswordRepeat),
			"Root");
}
