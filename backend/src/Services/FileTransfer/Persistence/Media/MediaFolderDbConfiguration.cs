﻿using Domain.Media;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Media;

internal sealed class MediaFolderDbConfiguration : IEntityTypeConfiguration<MediaFolder>
{
	public void Configure(EntityTypeBuilder<MediaFolder> builder)
	{
		builder.HasKey(f => f.Id);

		ConfigureProperties(builder);
		ConfigureRelations(builder);
	}

	private static void ConfigureProperties(EntityTypeBuilder<MediaFolder> builder)
	{
		builder.Property(f => f.Id).HasConversion(i => i.Value, g => new MediaFolderId(g));
		builder.Property(f => f.UserId).HasConversion(i => i.Value, g => new UserId(g));
	}

	private static void ConfigureRelations(EntityTypeBuilder<MediaFolder> builder)
	{
		builder.HasMany(f => f.MediaItems).WithOne().HasForeignKey(i => i.MediaFolderId);
	}
}