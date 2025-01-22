using Domain.Media;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Persistence;

public sealed class FileTransferDbContext : DbContext
{
	private readonly IPublisher _publisher;

	public FileTransferDbContext(DbContextOptions<FileTransferDbContext> options, IPublisher publisher) : base(options)
	{
		_publisher = publisher;
	}

	public DbSet<MediaFolder> MediaFolders { get; set; }
	public DbSet<MediaItem>   MediaItems   { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(DbSchemas.FileTransfer);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileTransferDbContext).Assembly);
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		OnBeforeSaving();

		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
	{
		var domainEvents = GatherDomainEvents();
		OnBeforeSaving();
		var result = await base.SaveChangesAsync(cancellationToken);
		await PublishDomainEventsAsync(domainEvents, cancellationToken);

		return result;
	}

	private void OnBeforeSaving()
	{
		var entries = ChangeTracker.Entries();
		foreach (var entry in entries)
		{
			if (entry.Entity is not Entity entity)
			{
				continue;
			}

			var now = DateTime.UtcNow;
			switch (entry.State)
			{
				case EntityState.Added:
					entity.CreatedUtc = now;

					break;
				case EntityState.Modified:
					entity.ModifiedUtc = now;

					break;
			}
		}
	}

	private List<IDomainEvent> GatherDomainEvents() =>
		ChangeTracker.Entries<Entity>()
			.Select(e => e.Entity)
			.Where(e => e.DomainEvents.Count != 0)
			.SelectMany(entity =>
			{
				var domainEvents = new List<IDomainEvent>(entity.DomainEvents);
				entity.ClearDomainEvents();

				return domainEvents;
			})
			.ToList();

	private async Task PublishDomainEventsAsync(List<IDomainEvent> domainEvents, CancellationToken cancellationToken)
	{
		foreach (var domainEvent in domainEvents)
		{
			await _publisher.Publish(domainEvent, cancellationToken);
		}
	}
}
