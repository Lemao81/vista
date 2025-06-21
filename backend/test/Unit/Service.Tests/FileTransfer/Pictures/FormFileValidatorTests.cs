using System.Net.Mime;
using Common.Presentation.Validators;
using Service.Tests.Utilities;

namespace Service.Tests.FileTransfer.Pictures;

public class FormFileValidatorTests
{
	private readonly FormFileValidator _classUnderTest;

	public FormFileValidatorTests()
	{
		_classUnderTest = new FormFileValidator();
	}

	[Fact]
	public async Task ValidateAsync_Given_valid_file_Should_return_valid()
	{
		// Arrange
		var file = new TestFormFile(MediaTypeNames.Image.Jpeg, "", 4000, "pic", "pic.jpg");

		// Act
		var result = await _classUnderTest.ValidateAsync(file, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task ValidateAsync_Given_invalid_file_Should_respond_errors()
	{
		// Arrange
		var file = new TestFormFile();

		// Act
		var result = await _classUnderTest.ValidateAsync(file, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
