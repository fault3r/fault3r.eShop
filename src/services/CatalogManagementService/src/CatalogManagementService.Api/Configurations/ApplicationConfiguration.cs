
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.UseCases.CreateItem;
using CatalogManagementService.Application.UseCases.DeleteItem;
using CatalogManagementService.Application.UseCases.GetItem;
using CatalogManagementService.Application.UseCases.GetItems;
using CatalogManagementService.Application.UseCases.UpdateItem;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Repositories;

namespace CatalogManagementService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            _logger.LogInformation("Configuring Application..");
            services.AddScoped<IRepository, MongoRepository>();
            services.AddScoped<IGetItemService, GetItemService>();
            services.AddScoped<IGetItemsService, GetItemsService>();
            services.AddScoped<ICreateItemService, CreateItemService>();
            services.AddScoped<IUpdateItemService, UpdateItemService>();
            services.AddScoped<IDeleteItemService, DeleteItemService>();
            _logger.LogInformation("Application configured successfully.");
            return services;
        }
    }
}