using System.Net;
using Domain.Media;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Service.Tests.FileTransfer;

public class UploadPictureEndpointTests : IClassFixture<FileTransferWebApplicationFactory>
{
	private readonly FileTransferWebApplicationFactory _webApplicationFactory;

	public UploadPictureEndpointTests(FileTransferWebApplicationFactory webApplicationFactory)
	{
		_webApplicationFactory = webApplicationFactory;
	}

	[Fact]
	public async Task When_UploadPicture_given_valid_image_should_persist_image()
	{
		// Arrange
		using var scope      = _webApplicationFactory.Services.CreateScope();
		var       dbContext  = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		var       httpClient = _webApplicationFactory.CreateClient();

		await using var stream      = File.OpenRead(Path.Combine("FileTransfer", "Files", "ph_600x400.png"));
		using var       formContent = new MultipartFormDataContent();
		formContent.Add(new StreamContent(stream), "file", "ph_600x400.png");

		// Pre-Assert
		Assert.Empty(dbContext.MediaFolders);
		Assert.Empty(dbContext.MediaItems);

		// Act
		var response = await httpClient.PostAsync("pictures", formContent);

		// Assert
		response.EnsureSuccessStatusCode();
		Assert.Single(dbContext.MediaFolders);
		Assert.Single(dbContext.MediaItems);
		var mediaFolder = dbContext.MediaFolders.Single();
		var mediaItem   = dbContext.MediaItems.First();
		Assert.Equal(MediaKind.Picture, mediaItem.MediaKind);
		Assert.Equal(MediaSizeKind.Original, mediaItem.MediaSizeKind);
		Assert.Equal(mediaFolder.Id, mediaItem.MediaFolderId);

		// TODO add object storage asserts
	}

	[Fact]
	public async Task When_UploadPicture_given_empty_file_should_return_bad_request()
	{
		// Arrange
		using var scope      = _webApplicationFactory.Services.CreateScope();
		var       httpClient = _webApplicationFactory.CreateClient();

		await using var stream      = File.OpenRead(Path.Combine("FileTransfer", "Files", "empty.png"));
		using var       formContent = new MultipartFormDataContent();
		formContent.Add(new StreamContent(stream), "file", "empty.png");

		// Act
		var response = await httpClient.PostAsync("pictures", formContent);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		var content = await response.Content.ReadAsStringAsync();
		Assert.Contains("'Length' must be greater than '0'", content);
	}
}
