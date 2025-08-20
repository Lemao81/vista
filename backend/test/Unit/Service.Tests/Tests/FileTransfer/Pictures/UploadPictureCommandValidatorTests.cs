using FileTransfer.Application.Pictures.Upload;
using FileTransfer.Domain.Media;
using NSubstitute;
using SharedKernel;

namespace Service.Tests.Tests.FileTransfer.Pictures;

public class UploadPictureCommandValidatorTests
{
	[Fact]
	public async Task ValidCommand_Should_ReturnValid()
	{
		// Arrange
		var options = new UploadMediaOptions
		{
			ValidPictureContentTypes   = ["image/png", "image/jpeg"],
			ValidPictureFileExtensions = ["png", "jpeg"],
			MaxPictureFileLengthKb     = 4096
		};

		var classUnderTest = new UploadPictureCommandValidator(options);
		var command        = new UploadPictureCommand(Substitute.For<Stream>(), "image/jpeg", new FileName("pic.jpeg"), new FileLength(2000));

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
			ValidPictureContentTypes   = ["image/png"],
			ValidPictureFileExtensions = ["png"],
			MaxPictureFileLengthKb     = 4096
		};

		var classUnderTest = new UploadPictureCommandValidator(options);
		var command        = new UploadPictureCommand(Substitute.For<Stream>(), "image/jpeg", new FileName("pic.jpeg"), new FileLength(0));

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
			ValidPictureContentTypes   = ["image/png", "image/jpeg"],
			ValidPictureFileExtensions = ["png", "jpeg"],
			MaxPictureFileLengthKb     = 1
		};

		var classUnderTest = new UploadPictureCommandValidator(options);
		var command        = new UploadPictureCommand(Substitute.For<Stream>(), "image/jpeg", new FileName("pic.jpeg"), new FileLength(5000));

		// Act
		var result = await classUnderTest.ValidateAsync(command, TestContext.Current.CancellationToken);

		// Assert
		await Verify(result);
	}
}
