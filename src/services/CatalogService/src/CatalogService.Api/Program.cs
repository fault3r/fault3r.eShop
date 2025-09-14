
using System;
using CatalogService.Api.Extensions;
using CatalogService.Infrastructure.Configurations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls(
            builder.Configuration["Urls"]?.Split(';') ??
            throw new Exception()
        );

        builder.Services.AddContextConfiguration(
            builder.Configuration.GetSection(nameof(ContextSettings)));

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
    }
}