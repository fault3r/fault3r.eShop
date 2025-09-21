
using System;
using CatalogService.Api.Extensions;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddContextConfiguration(
            builder.Configuration.GetSection(nameof(ContextSettings)));

        builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapGet("/", () =>
        {            
            return new { service = "CatalogService" };
        });

        app.MapControllers();

        string url = builder.Configuration.GetValue<string>("Url") ??
            throw new Exception(); 
            
        app.Run(url);
    }
}