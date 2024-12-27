using Domain.Media;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Media;

internal sealed class MediaItemDbConfiguration : IEntityTypeConfiguration<MediaItem>
{
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
	}
}
