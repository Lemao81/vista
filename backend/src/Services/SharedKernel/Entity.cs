namespace SharedKernel;

public abstract class Entity
{
	private readonly List<IDomainEvent> _domainEvents = [];

	public DateTime CreatedUtc { get; set; } = default;

	public DateTime? ModifiedUtc { get; set; }

	public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

	public bool HasDomainEvents => _domainEvents.Count > 0;

	public void ClearDomainEvents() => _domainEvents.Clear();

	protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}

#pragma warning disable SA1402
public abstract class Entity<T> : Entity
#pragma warning restore SA1402
{
	public abstract T Id { get; protected set; }
}
