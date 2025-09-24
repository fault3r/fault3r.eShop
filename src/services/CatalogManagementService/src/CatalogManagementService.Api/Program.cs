
using System;
using CatalogManagementService.Api.Extensions;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings))
    .Get<ApplicationSettings>() ??
    throw new NullReferenceException();

builder.Services.AddControllers();

var contextSettings = builder.Configuration.GetSection(nameof(ContextSettings))
    .Get<ContextSettings>() ??
    throw new NullReferenceException();
builder.Services.AddContextConfiguration(contextSettings);

builder.Services.AddScoped<ICatalogManagementRepository, CatalogManagementRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => appSettings.Name);

app.MapControllers();

app.Run(appSettings.Url);
