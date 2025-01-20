using System.Net.Mime;
using Service.Tests.Utilities;
using WebApi.Pictures.Upload;

namespace Service.Tests.FileTransfer.Pictures;

public class UploadPictureRequestValidatorTests
{
	private readonly UploadPictureRequestValidator _classUnderTest;

	public UploadPictureRequestValidatorTests()
	{
		_classUnderTest = new UploadPictureRequestValidator();
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
		Assert.True(result.IsValid);
	}

	[Fact]
	public async Task When_ValidateAsync_given_null_file_should_be_invalid()
	{
		// Arrange
		var request = new UploadPictureRequest(null);

		// Act
		var result = await _classUnderTest.ValidateAsync(request);

		// Assert
		Assert.False(result.IsValid);
		Assert.Contains("'File' must not be empty", result.Errors.First().ErrorMessage, StringComparison.Ordinal);
	}
}
