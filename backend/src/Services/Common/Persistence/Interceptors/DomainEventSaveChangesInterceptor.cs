using Lemao.UtilExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Common.Persistence.Interceptors;

public sealed class DomainEventSaveChangesInterceptor : SaveChangesInterceptor
{
	private readonly IPublisher          _publisher;
	private          List<IDomainEvent>? _domainEvents;

	public DomainEventSaveChangesInterceptor(IPublisher publisher)
	{
		_publisher = publisher;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		_domainEvents = GatherDomainEvents(eventData);

		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData      eventData,
		InterceptionResult<int> result,
		CancellationToken       cancellationToken = default)
	{
		_domainEvents = GatherDomainEvents(eventData);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
	{
		PublishDomainEventsAsync().GetAwaiter().GetResult();

		return base.SavedChanges(eventData, result);
	}

	public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
	{
		await PublishDomainEventsAsync(cancellationToken);

		return result;
	}

	private static List<IDomainEvent>? GatherDomainEvents(DbContextEventData eventData) =>
		eventData.Context?.ChangeTracker.Entries<IEntity>()
			.Select(e => e.Entity)
			.Where(e => e.HasDomainEvents)
			.SelectMany(entity =>
			{
				var domainEvents = entity.DomainEvents;
				entity.ClearDomainEvents();

				return domainEvents;
			})
			.ToList();

	private async Task PublishDomainEventsAsync(CancellationToken cancellationToken = default)
	{
		if (_domainEvents.IsNullOrEmpty())
		{
			return;
		}

		foreach (var domainEvent in _domainEvents)
		{
			await _publisher.Publish(domainEvent, cancellationToken);
		}

		_domainEvents.Clear();
		_domainEvents = null;
	}
}
