using System.Data.Common;
using Common.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Persistence;

public sealed class UnitOfWork<T> : IUnitOfWork
	where T : DbContext
{
	private readonly T _dbContext;

	public UnitOfWork(T dbContext)
	{
		_dbContext = dbContext;
	}

	public Task SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);

	public async Task<DbTransaction> BeginTransactionAsync() => (await _dbContext.Database.BeginTransactionAsync()).GetDbTransaction();
}
