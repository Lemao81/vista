using System.Data.Common;
using Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace FileTransfer.Infrastructure;

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
