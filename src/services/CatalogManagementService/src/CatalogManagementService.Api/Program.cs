
using System;
using CatalogManagementService.Api.Configurations;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings))
    .Get<ApplicationSettings>() ??
    throw new NullReferenceException(nameof(ApplicationSettings));

builder.Services.AddSerilogConfiguration(appSettings.Log);

builder.Services.AddVersioningConfiguration(appSettings.Version);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddMediatrConfiguration();

builder.Services.AddMongoContextConfiguration(builder.Configuration);

builder.Services.AddApplicationConfiguration();

builder.Services.AddRabbitmqConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", () =>
    $"Service Name: {appSettings.Name}\n" +
    $"Version: {appSettings.Version}");

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerService<Program>>();
    await logger.LogInformation($"✅ {appSettings.Name} is running..");
}

app.MapControllers();

app.Run(appSettings.Url);
