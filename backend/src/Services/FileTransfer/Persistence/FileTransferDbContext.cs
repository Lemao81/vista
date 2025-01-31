using Domain.Media;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class FileTransferDbContext : DbContext
{
	public FileTransferDbContext(DbContextOptions<FileTransferDbContext> options) : base(options)
	{
	}

	public DbSet<MediaFolder> MediaFolders { get; set; }
	public DbSet<MediaItem>   MediaItems   { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(DbSchemas.FileTransfer);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileTransferDbContext).Assembly);
	}
}
