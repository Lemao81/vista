namespace SharedKernel;

public interface IEntity
{
	DateTime CreatedUtc { get; set; }

	DateTime? ModifiedUtc { get; set; }

	IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

	bool HasDomainEvents { get; }

	void AddDomainEvent(IDomainEvent domainEvent);

	void ClearDomainEvents();
}
