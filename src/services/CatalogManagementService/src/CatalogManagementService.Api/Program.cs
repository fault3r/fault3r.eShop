
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

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddMediatrConfiguration();

builder.Services.AddMongoContextConfiguration(builder.Configuration);

builder.Services.AddScoped<IRepository, MongoRepository>();

builder.Services.AddScoped<IItemsService, ItemsService>();

builder.Services.AddRabbitmqConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", () => appSettings.Name);

app.MapControllers();

app.Run(appSettings.Url);
