namespace Domain.Abstractions;

public abstract class Entity<T>
{
	private readonly List<IDomainEvent> _domainEvents = [];

	public abstract T                                 Id           { get; protected set; }
	public          DateTime                          CreatedUtc   { get; private set; } = DateTime.UtcNow;
	public          DateTime?                         ModifiedUtc  { get; protected set; }
	public          IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

	protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
	protected void ClearDomainEvents()                        => _domainEvents.Clear();
}
