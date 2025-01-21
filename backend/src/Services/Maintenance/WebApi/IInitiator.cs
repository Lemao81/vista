namespace WebApi;

public interface IInitiator
{
	Task<bool> InitiateAsync(CancellationToken cancellationToken = default);
}
