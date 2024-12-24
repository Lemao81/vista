using Application;
using Domain;
using FluentValidation;
using Infrastructure;
using Persistence;
using Presentation;
using WebApi.Pictures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();
builder.Services.AddPresentationServices();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<AssemblyMarker>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseStatusCodePages();

app.MapPictureEndpoints();

app.UseHttpsRedirection();

app.Run();
