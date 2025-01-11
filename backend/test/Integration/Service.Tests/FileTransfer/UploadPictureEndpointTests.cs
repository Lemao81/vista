using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using Application.Utilities;
using Domain.Media;
using Domain.Users;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Service.Tests.Extensions;
using Service.Tests.WebApplicationFactories;
using Xunit.Abstractions;

namespace Service.Tests.FileTransfer;

public class UploadPictureEndpointTests : IClassFixture<FileTransferWebApplicationFactory>
{
	private readonly FileTransferWebApplicationFactory _webApplicationFactory;
	private readonly ITestOutputHelper                 _testOutputHelper;

	public UploadPictureEndpointTests(FileTransferWebApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper)
	{
		_webApplicationFactory = webApplicationFactory;
		_testOutputHelper      = testOutputHelper;
	}

	[Fact]
	public async Task When_UploadPicture_given_valid_image_should_persist_image()
	{
		// Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		using var       scope         = _webApplicationFactory.Services.CreateScope();
		await using var dbContext     = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		var             objectStorage = scope.ServiceProvider.GetRequiredService<IObjectStorage>();
		var             httpClient    = _webApplicationFactory.CreateClient();

		await using var stream        = File.OpenRead(Path.Combine("FileTransfer", "Files", "ph_600x400.png"));
		using var       formContent   = new MultipartFormDataContent();
		var             streamContent = new StreamContent(stream);
		streamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Png);
		formContent.Add(streamContent, "file", "ph_600x400.png");
		var userId = Guid.Parse("f6fac46a-ad79-44d8-8bc5-917e8cbad737");
		await objectStorage.CreateBucketAsync(StorageBucket.Media);

		// Pre-Assert
		Assert.Empty(dbContext.MediaFolders);
		Assert.Empty(dbContext.MediaItems);
		var itemsResult = await objectStorage.GetObjectItemsAsync(StorageBucket.Media, new StoragePrefix(userId));
		Assert.True(itemsResult.IsSuccess);
		Assert.Empty(itemsResult.Value!);

		// Act
		var response = await httpClient.PostAsync("pictures", formContent);
		await response.PrintContentAsync(_testOutputHelper);

		// Assert
		response.EnsureSuccessStatusCode();
		Assert.Single(dbContext.MediaFolders);
		Assert.Single(dbContext.MediaItems);
		var mediaFolder = dbContext.MediaFolders.Single();
		var mediaItem   = dbContext.MediaItems.First();
		Assert.Equal(MediaKind.Picture, mediaItem.MediaKind);
		Assert.Equal(MediaSizeKind.Original, mediaItem.MediaSizeKind);
		Assert.Equal(mediaFolder.Id, mediaItem.MediaFolderId);
		var fileName    = ApplicationHelper.GetStorageFileName(new FileName("ph_600x400.png"), mediaItem);
		var objectName  = StorageObjectName.CreateMediaName(fileName, new UserId(userId), mediaFolder.Id);
		var existResult = await objectStorage.ObjectExistAsync(StorageBucket.Media, objectName);
		Assert.True(existResult.IsSuccess);
		Assert.True(existResult.Value);
	}

	[Fact]
	public async Task When_UploadPicture_given_empty_file_should_return_bad_request()
	{
		// Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		using var scope      = _webApplicationFactory.Services.CreateScope();
		var       httpClient = _webApplicationFactory.CreateClient();

		await using var stream        = File.OpenRead(Path.Combine("FileTransfer", "Files", "empty.png"));
		using var       formContent   = new MultipartFormDataContent();
		var             streamContent = new StreamContent(stream);
		streamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Png);
		formContent.Add(streamContent, "file", "empty.png");

		// Act
		var response = await httpClient.PostAsync("pictures", formContent);
		await response.PrintContentAsync(_testOutputHelper);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		var content = await response.Content.ReadAsStringAsync();
		Assert.Contains("'Length' must be greater than '0'", content);
	}

	[Fact]
	public async Task When_UploadPicture_given_invalid_file_type_should_return_bad_request()
	{
		// Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		using var scope      = _webApplicationFactory.Services.CreateScope();
		var       httpClient = _webApplicationFactory.CreateClient();

		await using var stream        = File.OpenRead(Path.Combine("FileTransfer", "Files", "test.txt"));
		using var       formContent   = new MultipartFormDataContent();
		var             streamContent = new StreamContent(stream);
		streamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Plain);
		formContent.Add(streamContent, "file", "test.txt");

		// Act
		var response = await httpClient.PostAsync("pictures", formContent);
		await response.PrintContentAsync(_testOutputHelper);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		var content = await response.Content.ReadAsStringAsync();
		Assert.Contains("'Content Type' must be one of", content);
		Assert.Contains("'File Name' must have an extension of", content);
	}
}
