
using System;
using CatalogManagementService.Api.Configurations;
using CatalogManagementService.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetSection(nameof(ApplicationSettings))
    .Get<ApplicationSettings>() ??
    throw new NullReferenceException();

builder.Services.AddSerilogConfiguration(appSettings.Log);

//log
Console.WriteLine($"***{appSettings.Name} fetched the settings.");

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

//log
Console.WriteLine($"***{appSettings.Name} built successfully.");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.MapGet("/", () => appSettings.Name);

app.MapControllers();

//log
Console.WriteLine($"***{appSettings.Name} is running..");

app.Run(appSettings.Url);
