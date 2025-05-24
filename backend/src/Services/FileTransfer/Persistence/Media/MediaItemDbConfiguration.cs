using System.Text.Json;
using Common.Domain.Users;
using FileTransfer.Domain.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileTransfer.Persistence.Media;

internal sealed class MediaItemDbConfiguration : IEntityTypeConfiguration<MediaItem>
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new();

	public void Configure(EntityTypeBuilder<MediaItem> builder)
	{
		builder.HasKey(i => i.Id);

		ConfigureProperties(builder);
	}

	private static void ConfigureProperties(EntityTypeBuilder<MediaItem> builder)
	{
		builder.Property(i => i.Id).HasConversion(i => i.Value, g => new MediaItemId(g));
		builder.Property(i => i.UserId).HasConversion(i => i.Value, g => new UserId(g));
		builder.Property(i => i.MediaFolderId).HasConversion(i => i.Value, g => new MediaFolderId(g));
		builder.Property(i => i.MetaData)
			.HasConversion(
				d => JsonSerializer.Serialize(d, JsonSerializerOptions),
				i => JsonSerializer.Deserialize<Dictionary<string, object>>(i, JsonSerializerOptions) ?? new Dictionary<string, object>());
	}
}
