using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Maintenance.WebApi;

internal sealed class HealthCheck : IHealthCheck
{
	public static bool IsHealthy { get; set; }

	public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new()) =>
		IsHealthy ? Task.FromResult(HealthCheckResult.Healthy()) : Task.FromResult(HealthCheckResult.Unhealthy());
}
