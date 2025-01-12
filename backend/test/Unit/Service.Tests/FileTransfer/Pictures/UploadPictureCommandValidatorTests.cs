using System.Net.Http.Headers;
using Application.Pictures.Upload;
using Domain.Media;
using Microsoft.Extensions.Options;
using NSubstitute;
using SharedKernel;

namespace Service.Tests.FileTransfer.Pictures;

public class UploadPictureCommandValidatorTests
{
	[Fact]
	public async Task When_ValidateAsync_given_valid_command_should_return_valid()
	{
		// Arrange
		var options = Substitute.For<IOptions<UploadMediaOptions>>();
		options.Value.Returns(new UploadMediaOptions
		{
			ValidPictureContentTypes   = ["image/png", "image/jpeg"],
			ValidPictureFileExtensions = ["png", "jpeg"],
			MaxPictureFileLengthKb     = 4096
		});

		var classUnderTest = new UploadPictureCommandValidator(options);
		var command = new UploadPictureCommand(Substitute.For<Stream>(),
			new MediaTypeHeaderValue("image/jpeg"),
			new FileName("pic.jpeg"),
			new FileLength(2000));

		// Act
		var result = await classUnderTest.ValidateAsync(command);

		// Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	public async Task When_ValidateAsync_given_invalid_command_should_respond_errors()
	{
		// Arrange
		var options = Substitute.For<IOptions<UploadMediaOptions>>();
		options.Value.Returns(new UploadMediaOptions
		{
			ValidPictureContentTypes   = ["image/png"],
			ValidPictureFileExtensions = ["png"],
			MaxPictureFileLengthKb     = 4096
		});

		var classUnderTest = new UploadPictureCommandValidator(options);
		var command = new UploadPictureCommand(Substitute.For<Stream>(),
			new MediaTypeHeaderValue("image/jpeg"),
			new FileName("pic.jpeg"),
			new FileLength(0));

		// Act
		var result = await classUnderTest.ValidateAsync(command);

		// Assert
		Assert.False(result.IsValid);
		var errorString = result.ToString();
		Assert.Contains("'Content Type' must be one of", errorString);
		Assert.Contains("'File Name' must have an extension of", errorString);
		Assert.Contains("'File Length' must be greater than", errorString);
	}

	[Fact]
	public async Task When_ValidateAsync_given_exceed_file_length_should_respond_error()
	{
		// Arrange
		var options = Substitute.For<IOptions<UploadMediaOptions>>();
		options.Value.Returns(new UploadMediaOptions
		{
			ValidPictureContentTypes   = ["image/png", "image/jpeg"],
			ValidPictureFileExtensions = ["png", "jpeg"],
			MaxPictureFileLengthKb     = 1
		});

		var classUnderTest = new UploadPictureCommandValidator(options);
		var command = new UploadPictureCommand(Substitute.For<Stream>(),
			new MediaTypeHeaderValue("image/jpeg"),
			new FileName("pic.jpeg"),
			new FileLength(5000));

		// Act
		var result = await classUnderTest.ValidateAsync(command);

		// Assert
		Assert.False(result.IsValid);
		Assert.Contains("'File Length' must be less than or equal to", result.Errors.First().ErrorMessage);
	}
}
