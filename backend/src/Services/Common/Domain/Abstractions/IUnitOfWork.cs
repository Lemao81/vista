using System.Data.Common;

namespace Domain.Abstractions;

public interface IUnitOfWork
{
	Task                SaveChangesAsync(CancellationToken cancellationToken = default);
	Task<DbTransaction> BeginTransactionAsync();
}
