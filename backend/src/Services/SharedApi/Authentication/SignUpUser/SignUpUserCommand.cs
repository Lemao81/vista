using Common.Application.Abstractions.Command;

namespace SharedApi.Authentication.SignUpUser;

public sealed record SignUpUserCommand(
	string UserName,
	string Email,
	string Password,
	string PasswordRepeat,
	string UserNameParameterName,
	string EmailParameterName,
	string PasswordParameterName,
	string PasswordRepeatParameterName,
	string RootParameterName) : ITransactionalCommand;
