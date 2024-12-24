namespace Domain.Abstractions;

public abstract class Entity<T>
{
	public abstract T Id { get; protected set; }
	public DateTime CreatedUtc { get; } = DateTime.UtcNow;
	public DateTime? ModifiedUtc { get; protected set; }
	public List<IDomainEvent> DomainEvents { get; } = [];

	protected void RaiseDomainEvent(IDomainEvent domainEvent) => DomainEvents.Add(domainEvent);
	protected void ClearDomainEvents() => DomainEvents.Clear();
}
