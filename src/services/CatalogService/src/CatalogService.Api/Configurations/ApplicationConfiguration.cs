
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Application.UseCases.CreateItem;
using CatalogService.Application.UseCases.DeleteItem;
using CatalogService.Application.UseCases.GetItem;
using CatalogService.Application.UseCases.GetItems;
using CatalogService.Application.UseCases.UpdateItem;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Repositories;

namespace CatalogService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring Application..");
                services.AddScoped<IRepository, MongoRepository>();
                services.AddScoped<IGetItemService, GetItemService>();
                services.AddScoped<IGetItemsService, GetItemsService>();
                services.AddScoped<ICreateItemService, CreateItemService>();
                services.AddScoped<IUpdateItemService, UpdateItemService>();
                services.AddScoped<IDeleteItemService, DeleteItemService>();
                _logger.LogInformation("Application configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure Application settings!");
                throw new InvalidOperationException(nameof(Program));                
            }
        }
    }
}