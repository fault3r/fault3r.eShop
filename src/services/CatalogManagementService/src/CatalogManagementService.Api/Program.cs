
using System;
using CatalogManagementService.Api.Configurations;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.Services;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings))
    .Get<ApplicationSettings>() ??
    throw new NullReferenceException();

builder.Services.AddVersioningConfiguration(appSettings.Version);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings))
    .Get<JwtSettings>() ??
    throw new NullReferenceException();
builder.Services.AddJwtConfiguration(jwtSettings);

builder.Services.AddMediatrConfiguration();

var mongoContextSettings = builder.Configuration.GetSection(nameof(MongoSettings))
    .Get<MongoSettings>() ??
    throw new NullReferenceException();
builder.Services.AddMongoContextConfiguration(mongoContextSettings);

builder.Services.AddScoped<IItemsRepository, ItemsRepository>();

builder.Services.AddScoped<IItemsService, ItemsService>();

var rabbitmqSettings = builder.Configuration.GetSection(nameof(RabbitmqSettings))
    .Get<RabbitmqSettings>() ??
    throw new NullReferenceException();
builder.Services.AddRabbitmqConfiguration(rabbitmqSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => appSettings.Name);

app.MapControllers();

app.Run(appSettings.Url);
