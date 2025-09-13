using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongodbSettings>(
    builder.Configuration.GetSection(nameof(MongodbSettings)));

builder.Services.AddSingleton<CatalogContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () =>
{
    return new { service = "CatalogService" };
});

app.Run();