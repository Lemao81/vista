using System.Data.Common;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
	private readonly FileTransferDbContext _dbContext;

	public UnitOfWork(FileTransferDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);

	public async Task<DbTransaction> BeginTransactionAsync() => (await _dbContext.Database.BeginTransactionAsync()).GetDbTransaction();
}
