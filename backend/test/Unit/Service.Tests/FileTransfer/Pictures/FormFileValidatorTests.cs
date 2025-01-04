﻿using System.Net.Mime;
using Service.Tests.Utilities;
using WebApi.Pictures.Upload;

namespace Service.Tests.FileTransfer.Pictures;

public class FormFileValidatorTests
{
	private readonly FormFileValidator _classUnderTest;

	public FormFileValidatorTests()
	{
		_classUnderTest = new FormFileValidator();
	}

	[Fact]
	public async Task When_ValidateAsync_given_valid_file_should_return_valid()
	{
		// Arrange
		var file = new TestFormFile(MediaTypeNames.Image.Jpeg, "", 4000, "pic", "pic.jpg");

		// Act
		var result = await _classUnderTest.ValidateAsync(file);

		// Assert
		Assert.True(result.IsValid);
	}

	[Fact]
	public async Task When_ValidateAsync_given_invalid_file_should_respond_errors()
	{
		// Arrange
		var file = new TestFormFile();

		// Act
		var result = await _classUnderTest.ValidateAsync(file);

		// Assert
		Assert.False(result.IsValid);
		var errorString = result.ToString();
		Assert.Contains("'Content Type' must not be empty", errorString);
		Assert.Contains("'Content Type' not valid", errorString);
		Assert.Contains("'File Name' must not be empty", errorString);
		Assert.Contains("'File Name' must have an extension", errorString);
		Assert.Contains("'Length' must be greater than '0'", errorString);
	}
}