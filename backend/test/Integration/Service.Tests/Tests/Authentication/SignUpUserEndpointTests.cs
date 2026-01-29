using System.Net.Http.Json;
using Authentication.Domain.Constants;
using Authentication.Domain.User;
using Authentication.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.Tests.Extensions;
using Service.Tests.WebApplicationFactories;
using SharedApi.Authentication.SignUpUser;
using Tests.Common;

namespace Service.Tests.Tests.Authentication;

public class SignUpUserEndpointTests : IClassFixture<AuthenticationWebApplicationFactory>, IAsyncDisposable
{
	private readonly AuthenticationWebApplicationFactory _webApplicationFactory;
	private readonly ITestOutputHelper                   _testOutputHelper;

	public SignUpUserEndpointTests(AuthenticationWebApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper)
	{
		_webApplicationFactory = webApplicationFactory;
		_testOutputHelper      = testOutputHelper;
	}

	[Fact]
	public async Task ValidSignUp_Should_CreateUser()
	{
		// Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		using var       httpClient = _webApplicationFactory.CreateClient();
		using var       scope      = _webApplicationFactory.Services.CreateScope();
		await using var dbContext  = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

		var request = new SignUpUserRequest("user", "test@test.com", "Test01!", "Test01!");

		// Pre-Assert
		Assert.Empty(dbContext.Users);
		Assert.Empty(dbContext.UserRoles);

		// Act
		var response = await httpClient.PostAsJsonAsync("api/signUp", request, TestContext.Current.CancellationToken);
		await response.PrintContentAsync(_testOutputHelper);

		// Assert
		await Verify(response, VerifySettingsFactory.ScrubGuidsSettings);
		Assert.Single(dbContext.Users);
		Assert.Single(dbContext.UserRoles);
		var user = dbContext.Users.Single();
		Assert.Equal("test@test.com", user.Email);
		var userRole = dbContext.UserRoles.Single();
		var role     = dbContext.Roles.Single(r => r.Id == userRole.RoleId);
		Assert.Equal(UserRoles.Viewer, role.Name);
	}

	[Fact]
	public async Task InvalidSignUp_Should_ReturnBadRequest()
	{
		// Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		using var httpClient = _webApplicationFactory.CreateClient();

		var request = new SignUpUserRequest(string.Empty, string.Empty, string.Empty, string.Empty);

		// Act
		var response = await httpClient.PostAsJsonAsync("api/signUp", request, TestContext.Current.CancellationToken);
		await response.PrintContentAsync(_testOutputHelper);

		// Assert
		await Verify(response, VerifySettingsFactory.ScrubGuidsSettings);
	}

	[Fact]
	public async Task ExistingEmailAndName_Should_ReturnBadRequest()
	{
		// Arrange
		_webApplicationFactory.TestOutputHelper = _testOutputHelper;
		using var       httpClient   = _webApplicationFactory.CreateClient();
		using var       scope        = _webApplicationFactory.Services.CreateScope();
		await using var dbContext    = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
		var             userManager  = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
		var             createResult = await userManager.CreateAsync(AppUser.Create("test", "test@test.com"));
		Assert.True(createResult.Succeeded);
		await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

		var request = new SignUpUserRequest("test", "test@test.com", "Test01!", "Test01!");

		// Act
		var response = await httpClient.PostAsJsonAsync("api/signUp", request, TestContext.Current.CancellationToken);
		await response.PrintContentAsync(_testOutputHelper);

		// Assert
		await Verify(response, VerifySettingsFactory.ScrubGuidsSettings);
	}

	public async ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		using var       scope     = _webApplicationFactory.Services.CreateScope();
		await using var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
		await dbContext.Users.ExecuteDeleteAsync(TestContext.Current.CancellationToken);
	}
}
