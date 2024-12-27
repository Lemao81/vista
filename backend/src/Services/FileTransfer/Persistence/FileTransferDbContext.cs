using Domain.Media;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class FileTransferDbContext : DbContext
{
	public DbSet<MediaFolder> MediaFolders { get; set; }
	public DbSet<MediaItem>   MediaItems   { get; set; }

	public FileTransferDbContext(DbContextOptions<FileTransferDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasDefaultSchema(DbSchemas.FileTransfer);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileTransferDbContext).Assembly);
	}
}
