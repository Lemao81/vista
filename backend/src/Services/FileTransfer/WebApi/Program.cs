using Application;
using Domain;
using Domain.ValueObjects;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Presentation;
using WebApi;
using WebApi.Pictures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
	if (context.HttpContext.Items.TryGetValue(HttpContextItemKeys.ErrorCode, out var code) && code is ErrorCode errorCode)
	{
		context.ProblemDetails.Extensions.Add(ProblemDetailsExtensionKeys.ErrorCode, errorCode.Value);
	}
});

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(includeInternalTypes: true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseStatusCodePages();

app.MapPictureEndpoints();

app.UseHttpsRedirection();

await MigrateDatabaseAsync(app);

app.Run();

return;

async Task MigrateDatabaseAsync(WebApplication webApplication)
{
	using var scope     = webApplication.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
	var       dbContext = scope.ServiceProvider.GetRequiredService<FileTransferDbContext>();
	await dbContext.Database.MigrateAsync();
}
