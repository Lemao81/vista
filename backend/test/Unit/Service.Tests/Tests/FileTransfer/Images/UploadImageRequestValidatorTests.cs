using System.Net.Mime;
using Common.Presentation.Validators;
using FileTransfer.Presentation.Images.Upload;
using Service.Tests.Utilities;
using SharedApi.FileTransfer.UploadImage;

namespace Service.Tests.Tests.FileTransfer.Images;

public class UploadImageRequestValidatorTests
{
	private readonly UploadImageRequestValidator _classUnderTest;

	public UploadImageRequestValidatorTests()
	{
		_classUnderTest = new UploadImageRequestValidator(new FormFileValidator());
	}

	[Fact]
	public async Task ValidFile_Should_ReturnValid()
	{
		// Arrange
		var file    = new TestFormFile(MediaTypeNames.Image.Jpeg, "", 4000, "pic", "pic.jpg");
		var request = new UploadImageRequest(file);

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task NullFile_Should_BeInvalid()
	{
		// Arrange
		var request = new UploadImageRequest(null);

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
