using System.Net.Mime;
using Common.Presentation.Validators;
using FileTransfer.Presentation.Pictures.Upload;
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
	public async Task ValidateAsync_Given_valid_file_Should_return_valid()
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
	public async Task ValidateAsync_Given_null_file_Should_be_invalid()
	{
		// Arrange
		var request = new UploadPictureRequest(null);

		// Act
		var result = await _classUnderTest.ValidateAsync(request);

		// Assert
		await Verify(result);
	}
}
