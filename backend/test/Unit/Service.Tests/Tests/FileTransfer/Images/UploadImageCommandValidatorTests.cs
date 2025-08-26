using FileTransfer.Application.Images.Upload;
using FileTransfer.Domain.Media;
using NSubstitute;
using SharedKernel;

namespace Service.Tests.Tests.FileTransfer.Images;

public class UploadImageCommandValidatorTests
{
	[Fact]
	public async Task ValidCommand_Should_ReturnValid()
	{
		// Arrange
		var options = new UploadMediaOptions
		{
			ValidImageContentTypes   = ["image/png", "image/jpeg"],
			ValidImageFileExtensions = ["png", "jpeg"],
			MaxImageFileLengthKb     = 4096
		};

		var classUnderTest = new UploadImageCommandValidator(options);
		var command        = new UploadImageCommand(Substitute.For<Stream>(), "image/jpeg", new FileName("pic.jpeg"), new FileLength(2000));

		// Act
		var result = await classUnderTest.ValidateAsync(command, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task InvalidCommand_Should_RespondErrors()
	{
		// Arrange
		var options = new UploadMediaOptions
		{
			ValidImageContentTypes   = ["image/png"],
			ValidImageFileExtensions = ["png"],
			MaxImageFileLengthKb     = 4096
		};

		var classUnderTest = new UploadImageCommandValidator(options);
		var command        = new UploadImageCommand(Substitute.For<Stream>(), "image/jpeg", new FileName("pic.jpeg"), new FileLength(0));

		// Act
		var result = await classUnderTest.ValidateAsync(command, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task? ExceedFileLength_Should_RespondError()
	{
		// Arrange
		var options = new UploadMediaOptions
		{
			ValidImageContentTypes   = ["image/png", "image/jpeg"],
			ValidImageFileExtensions = ["png", "jpeg"],
			MaxImageFileLengthKb     = 1
		};

		var classUnderTest = new UploadImageCommandValidator(options);
		var command        = new UploadImageCommand(Substitute.For<Stream>(), "image/jpeg", new FileName("pic.jpeg"), new FileLength(5000));

		// Act
		var result = await classUnderTest.ValidateAsync(command, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
