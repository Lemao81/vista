using System.Data.Common;

namespace Application.Abstractions;

public interface IUnitOfWork
{
	Task                SaveChangesAsync(CancellationToken cancellationToken = default);
	Task<DbTransaction> BeginTransactionAsync();
}
