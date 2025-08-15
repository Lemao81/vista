using FileTransfer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests.Utilities;

public sealed class FileTransferDbContextFixture : IAsyncDisposable
{
	public FileTransferDbContextFixture()
	{
		var dbOptionsBuilder = new DbContextOptionsBuilder<FileTransferDbContext>();
		dbOptionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
		DbContext = new FileTransferDbContext(dbOptionsBuilder.Options);
	}

	public FileTransferDbContext DbContext { get; }

	public async ValueTask DisposeAsync() => await DbContext.DisposeAsync();
}
