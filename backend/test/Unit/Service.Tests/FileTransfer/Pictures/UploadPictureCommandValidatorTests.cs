using Application.Pictures.Upload;
using Domain.Media;
using NSubstitute;
using SharedKernel;

namespace Service.Tests.FileTransfer.Pictures;

public class UploadPictureCommandValidatorTests
{
	[Fact]
	public async Task When_ValidateAsync_given_valid_command_should_return_valid()
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
		var result = await classUnderTest.ValidateAsync(command);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task When_ValidateAsync_given_invalid_command_should_respond_errors()
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
		var result = await classUnderTest.ValidateAsync(command);

		// Assert
		await Verify(result);
	}

	[Fact]
	public async Task When_ValidateAsync_given_exceed_file_length_should_respond_error()
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
		var result = await classUnderTest.ValidateAsync(command);

		// Assert
		await Verify(result);
	}
}
