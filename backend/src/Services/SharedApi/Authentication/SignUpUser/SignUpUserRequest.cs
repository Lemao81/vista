namespace SharedApi.Authentication.SignUpUser;

public sealed record SignUpUserRequest(string UserName, string Email, string Password, string PasswordRepeat);
