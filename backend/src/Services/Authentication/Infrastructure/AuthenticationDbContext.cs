using Authentication.Domain.User;
using Common.Persistence.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure;

public sealed class AuthenticationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
	public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.HasDefaultSchema(DbSchemas.Authentication);
		builder.ApplyConfigurationsFromAssembly(typeof(AuthenticationDbContext).Assembly);

		builder.Entity<AppUser>().ToTable("users", DbSchemas.Authentication);
		builder.Entity<IdentityRole<Guid>>().ToTable("roles", DbSchemas.Authentication);
		builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims", DbSchemas.Authentication);
		builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims", DbSchemas.Authentication);
		builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins", DbSchemas.Authentication);
		builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles", DbSchemas.Authentication);
		builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens", DbSchemas.Authentication);
	}
}
