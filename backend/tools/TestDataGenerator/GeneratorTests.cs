using System.Diagnostics.CodeAnalysis;
using Domain.Media;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Persistence;

// ReSharper disable Xunit.XunitTestWithConsoleOutput

namespace TestDataGenerator;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped")]
public class GeneratorTests
{
	[Fact(Skip = "TestDataGenerator")]
	public async Task ClearMediaData()
	{
		await GetDbContext().MediaFolders.ExecuteDeleteAsync();
	}

	[Fact(Skip = "TestDataGenerator")]
	public async Task AddMediaFolders()
	{
		var dbContext = GetDbContext();

		var userId = new UserId(Guid.NewGuid());
		var mediaFolders = Enumerable.Range(0, 10)
			.Select(index =>
			{
				var       mediaFolder = MediaFolder.Create(userId);
				MediaItem mediaItem;
				if (index % 2 == 0)
				{
					mediaItem = MediaItem.Create(mediaFolder.Id, userId, MediaKind.Picture, MediaSizeKind.Original);
				}
				else
				{
					mediaItem = MediaItem.Create(mediaFolder.Id, userId, MediaKind.Video, MediaSizeKind.Original);
					mediaItem.AddMetaData("DurationSec", Random.Shared.NextInt64(5, 31));
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
		var dbPassword    = configBuilder.Build().GetSection("DbPassword").Value;

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
