using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Persistence.Interceptors;

public sealed class AuditDateSaveChangesInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		SetAuditDates(eventData);

		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData      eventData,
	                                                                      InterceptionResult<int> result,
	                                                                      CancellationToken       cancellationToken = new())
	{
		SetAuditDates(eventData);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void SetAuditDates(DbContextEventData eventData)
	{
		if (eventData.Context is null)
		{
			return;
		}

		var entries = eventData.Context.ChangeTracker.Entries();
		foreach (var entry in entries)
		{
			if (entry.Entity is not Entity entity)
			{
				continue;
			}

			var now = DateTime.UtcNow;
			switch (entry.State)
			{
				case EntityState.Added:
					entity.CreatedUtc = now;

					break;
				case EntityState.Modified:
					entity.ModifiedUtc = now;

					break;
			}
		}
	}
}
