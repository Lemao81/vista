namespace Maintenance.WebApi;

public interface IInitiator
{
	bool       IsEnabled();
	Task<bool> InitiateAsync(CancellationToken cancellationToken = default);
}
