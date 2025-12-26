using System.Globalization;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.User;
using Authentication.Infrastructure;
using Authentication.Presentation;
using Authentication.WebApi.Extensions;
using Common.Persistence.Utilities;
using Common.WebApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Serilog;

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console(formatProvider: CultureInfo.InvariantCulture).CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommonAppSettings();

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddCommonServices();
builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Environment, builder.Logging);
builder.Services.AddDatabasePersistenceServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
	{
		options.User.RequireUniqueEmail = true;
	})
	.AddEntityFrameworkStores<AuthenticationDbContext>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddSerilog((sp, configuration) => configuration.ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(sp));

var app = builder.Build();

app.UseExceptionHandler();
app.MapHealthChecks("/api/health");

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseStatusCodePages();

app.MapEndpoints();

app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
await PersistenceHelper.AwaitDatabaseConnectionAsync(app.Services, logger);
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
	await app.MigrateDatabaseAsync();
}

await app.AddUserRolesAsync(logger);

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();
