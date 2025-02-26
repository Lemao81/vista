using System.Net.Mime;
using Presentation.Pictures.Upload;
using Presentation.Validators;
using Service.Tests.Utilities;

namespace Service.Tests.FileTransfer.Pictures;

public class UploadPictureRequestValidatorTests
{
	private readonly UploadPictureRequestValidator _classUnderTest;

	public UploadPictureRequestValidatorTests()
	{
		_classUnderTest = new UploadPictureRequestValidator(new FormFileValidator());
	}

	[Fact]
	public async Task When_ValidateAsync_given_valid_file_should_return_valid()
	{
		// Arrange
		var file    = new TestFormFile(MediaTypeNames.Image.Jpeg, "", 4000, "pic", "pic.jpg");
		var request = new UploadPictureRequest(file);

		// Act
		var result = await _classUnderTest.ValidateAsync(request);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task When_ValidateAsync_given_null_file_should_be_invalid()
	{
		// Arrange
		var request = new UploadPictureRequest(null);

		// Act
		var result = await _classUnderTest.ValidateAsync(request);

		// Assert
		await Verify(result);
	}
}
