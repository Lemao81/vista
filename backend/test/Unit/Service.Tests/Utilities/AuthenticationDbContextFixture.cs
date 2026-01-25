using Authentication.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests.Utilities;

public sealed class AuthenticationDbContextFixture : IAsyncDisposable
{
	public AuthenticationDbContextFixture()
	{
		var dbOptionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
		dbOptionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
		DbContext = new AuthenticationDbContext(dbOptionsBuilder.Options);
	}

	public AuthenticationDbContext DbContext { get; }

	public async ValueTask DisposeAsync() => await DbContext.DisposeAsync();
}
