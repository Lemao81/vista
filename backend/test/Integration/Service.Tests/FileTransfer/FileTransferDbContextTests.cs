using FileTransfer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests.FileTransfer;

public class FileTransferDbContextTests
{
	[Fact]
	public async Task GenerateScript_Should_generate_proper_schema()
	{
		// Arrange
		var dbOptionsBuilder = new DbContextOptionsBuilder<FileTransferDbContext>();
		dbOptionsBuilder.UseNpgsql();
		await using var context = new FileTransferDbContext(dbOptionsBuilder.Options);

		// Act
		var script = context.Database.GenerateCreateScript();

		// Assert
		await Verify(script);
	}
}
