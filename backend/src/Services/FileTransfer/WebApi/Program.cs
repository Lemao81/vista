using Application;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Presentation;
using WebApi.Pictures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddPresentationServices();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
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
