﻿namespace SharedKernel;

public abstract class Entity
{
	private readonly List<IDomainEvent> _domainEvents = [];

	public DateTime                          CreatedUtc   { get; private set; } = DateTime.UtcNow;
	public DateTime?                         ModifiedUtc  { get; protected set; }
	public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

	protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

	public void ClearDomainEvents() => _domainEvents.Clear();
}

public abstract class Entity<T> : Entity
{
	public abstract T Id { get; protected set; }
}
