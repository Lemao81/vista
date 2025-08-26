using System.Net.Mime;
using Common.Presentation.Validators;
using Service.Tests.Utilities;

namespace Service.Tests.Tests.FileTransfer.Images;

public class FormFileValidatorTests
{
	private readonly FormFileValidator _classUnderTest;

	public FormFileValidatorTests()
	{
		_classUnderTest = new FormFileValidator();
	}

	[Fact]
	public async Task ValidFile_Should_ReturnValid()
	{
		// Arrange
		var file = new TestFormFile(MediaTypeNames.Image.Jpeg, "", 4000, "pic", "pic.jpg");

		// Act
		var result = await _classUnderTest.ValidateAsync(file, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task InvalidFile_Should_RespondErrors()
	{
		// Arrange
		var file = new TestFormFile();

		// Act
		var result = await _classUnderTest.ValidateAsync(file, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
