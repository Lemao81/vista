using System.Data.Common;

namespace Common.Application.Abstractions;

public interface IUnitOfWork
{
	Task                SaveChangesAsync(CancellationToken cancellationToken = default);
	Task<DbTransaction> BeginTransactionAsync();
}
