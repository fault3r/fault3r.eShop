
using System;
using CatalogManagementService.Api.Extensions;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.WebHost.UseUrls(
            builder.Configuration["Urls"]?.Split(';') ??
            throw new Exception()
        );

        builder.Services.AddContextConfiguration(
            builder.Configuration.GetSection(nameof(ContextSettings)));

        builder.Services.AddScoped<ICatalogManagementRepository, CatalogManagementRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapGet("/", () =>
        {            
            return new { service = "CatalogManagementService" };
        });

        app.MapControllers();

        app.Run();
    }
}