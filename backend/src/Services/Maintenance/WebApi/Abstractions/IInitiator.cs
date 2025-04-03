namespace Maintenance.WebApi.Abstractions;

public interface IInitiator
{
	bool       IsEnabled();
	Task<bool> InitiateAsync(CancellationToken cancellationToken = default);
}
