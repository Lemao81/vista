using System.Net.Mime;
using Common.Presentation.Validators;
using FileTransfer.Presentation.Pictures.Upload;
using Service.Tests.Utilities;

namespace Service.Tests.Tests.FileTransfer.Pictures;

public class UploadPictureRequestValidatorTests
{
	private readonly UploadPictureRequestValidator _classUnderTest;

	public UploadPictureRequestValidatorTests()
	{
		_classUnderTest = new UploadPictureRequestValidator(new FormFileValidator());
	}

	[Fact]
	public async Task ValidFile_Should_ReturnValid()
	{
		// Arrange
		var file    = new TestFormFile(MediaTypeNames.Image.Jpeg, "", 4000, "pic", "pic.jpg");
		var request = new UploadPictureRequest(file);

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task NullFile_Should_BeInvalid()
	{
		// Arrange
		var request = new UploadPictureRequest(null);

		// Act
		var result = await _classUnderTest.ValidateAsync(request, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
