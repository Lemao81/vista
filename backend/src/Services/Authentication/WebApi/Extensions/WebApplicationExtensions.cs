using Authentication.Domain.Constants;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.WebApi.Extensions;

internal static class WebApplicationExtensions
{
	public static async Task MigrateDatabaseAsync(this WebApplication app)
	{
		await using var scope     = app.Services.CreateAsyncScope();
		await using var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
		await dbContext.Database.MigrateAsync(app.Lifetime.ApplicationStopped);
	}

	public static async Task AddUserRolesAsync(this WebApplication app, ILogger logger)
	{
		await using var scope       = app.Services.CreateAsyncScope();
		await using var dbContext   = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
		var             roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
		await AddUserRoleIfNotExistAsync(logger, roleManager, UserRoles.Admin);
		await AddUserRoleIfNotExistAsync(logger, roleManager, UserRoles.Creator);
		await AddUserRoleIfNotExistAsync(logger, roleManager, UserRoles.Viewer);
	}

	private static async Task AddUserRoleIfNotExistAsync(ILogger logger, RoleManager<IdentityRole<Guid>> roleManager, string role)
	{
		if (await roleManager.RoleExistsAsync(role))
		{
			return;
		}

		var result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
		if (!result.Succeeded)
		{
			logger.LogError("Adding role '{UserRole}' failed | Errors={Errors}", role, result.Errors);
		}
	}
}
