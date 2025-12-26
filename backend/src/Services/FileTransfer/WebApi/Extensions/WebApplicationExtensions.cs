using Common.Presentation.Constants;
using FileTransfer.Infrastructure;
using FileTransfer.Presentation.Images.Upload;
using Microsoft.EntityFrameworkCore;

namespace FileTransfer.WebApi.Extensions;

internal static class WebApplicationExtensions
{
	public static async Task MigrateDatabaseAsync(this WebApplication app)
	{
		await using var scope     = app.Services.CreateAsyncScope();
		await using var dbContext = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
		await dbContext.Database.MigrateAsync(app.Lifetime.ApplicationStopped);
	}

	public static void MapEndpoints(this WebApplication app)
	{
		var apiGroup    = app.MapGroup(Routes.Api).RequireAuthorization();
		var imagesGroup = apiGroup.MapGroup(Routes.Images);
		imagesGroup.MapUploadImageEndpoint();
	}
}
