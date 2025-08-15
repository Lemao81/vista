using FileTransfer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FileTransfer.WebApi.Extensions;

internal static class WebApplicationExtensions
{
	public static async Task MigrateDatabaseAsync(this WebApplication app)
	{
		await using var scope     = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
		await using var dbContext = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		await dbContext.Database.MigrateAsync();
	}
}
