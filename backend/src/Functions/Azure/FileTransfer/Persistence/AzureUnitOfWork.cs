using System.Data.Common;
using Application.Abstractions;

namespace FileTransfer.Persistence;

public class AzureUnitOfWork : IUnitOfWork
{
	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<DbTransaction> BeginTransactionAsync()
	{
		throw new NotImplementedException();
	}
}
