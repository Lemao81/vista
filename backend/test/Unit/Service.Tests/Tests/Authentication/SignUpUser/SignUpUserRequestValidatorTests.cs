using Authentication.Presentation.SignUpUser;
using SharedApi.Authentication.SignUpUser;

namespace Service.Tests.Tests.Authentication.SignUpUser;

public class SignUpUserRequestValidatorTests
{
	private readonly SignUpUserRequestValidator _classUnderTest;

	public SignUpUserRequestValidatorTests()
	{
		_classUnderTest = new SignUpUserRequestValidator();
	}

	[Fact]
	public async Task ValidRequest_Should_ReturnValid()
	{
		// Arrange
		var request = new SignUpUserRequest("user", "test@test.com", "any", "any");

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task EmptyInputs_Should_BeInvalid()
	{
		// Arrange
		var request = new SignUpUserRequest(string.Empty, string.Empty, string.Empty, string.Empty);

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task InvalidEmail_Should_BeInvalid()
	{
		// Arrange
		var request = new SignUpUserRequest("user", "test.com", "any", "any");

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task UnequalPasswordRepeat_Should_BeInvalid()
	{
		// Arrange
		var request = new SignUpUserRequest("user", "test@test.com", "any", "other");

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
