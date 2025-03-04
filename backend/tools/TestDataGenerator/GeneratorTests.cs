using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Bogus;
using Common.Domain.Users;
using Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Persistence;

// ReSharper disable Xunit.XunitTestWithConsoleOutput

namespace TestDataGenerator;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped")]
public class GeneratorTests
{
	private readonly (string Extension, string MediaType)[] _pictureMetadata = [("png", "image/png"), ("jpg", "image/jpeg"), ("jpeg", "image/jpeg")];
	private readonly (string Extension, string MediaType)[] _videoMetadata   = [("mp4", "video/mp4"), ("avi", "video/x-msvideo"), ("mkv", "video/x-matroska")];
	private readonly Faker                                  _faker           = new();

	[Fact(Skip = "TestDataGenerator")]
	public async Task ClearMediaData()
	{
		await using var dbContext = GetDbContext();
		await dbContext.MediaFolders.ExecuteDeleteAsync();
	}

	[Fact(Skip = "TestDataGenerator")]
	public async Task AddMediaFolders()
	{
		await using var dbContext = GetDbContext();

		var userId = new UserId(Guid.NewGuid());
		var mediaFolders = Enumerable.Range(0, 10)
			.Select(index =>
			{
				var mediaKind     = index % 2 == 0 ? MediaKind.Picture : MediaKind.Video;
				var metadataArray = index % 2 == 0 ? _pictureMetadata : _videoMetadata;
				var (extension, mediaType) = metadataArray[Random.Shared.Next(metadataArray.Length)];

				var mediaFolder = MediaFolder.Create(userId, _faker.System.FileName(extension));
				var mediaItem   = MediaItem.Create(mediaFolder.Id, userId, mediaKind, MediaSizeKind.Original, mediaType);
				if (mediaKind == MediaKind.Video)
				{
					mediaItem.AddMetaData("DurationSec", RandomNumberGenerator.GetInt32(5, 31));
				}

				mediaFolder.AddMediaItem(mediaItem);

				return mediaFolder;
			})
			.ToList();

		await dbContext.AddRangeAsync(mediaFolders);
		await dbContext.SaveChangesAsync();
	}

	private static FileTransferDbContext GetDbContext()
	{
		var configBuilder = new ConfigurationBuilder().AddUserSecrets<GeneratorTests>();
		var dbPassword    = configBuilder.Build().GetSection("Database:Password").Value;

		var dbBuilder = new DbContextOptionsBuilder<FileTransferDbContext>();
		dbBuilder.UseNpgsql(new NpgsqlDataSourceBuilder
			{
				ConnectionStringBuilder =
				{
					Host     = "localhost",
					Database = "vista_file_transfer",
					Username = "sa",
					Password = dbPassword
				}
			}.Build())
			.UseSnakeCaseNamingConvention();

		return new FileTransferDbContext(dbBuilder.Options);
	}
}
