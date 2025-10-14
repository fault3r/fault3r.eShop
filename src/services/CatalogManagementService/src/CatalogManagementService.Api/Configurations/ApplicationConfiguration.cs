
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
            services.AddScoped<IRepository, MongoRepository>();
            services.AddScoped<IGetItemService, GetItemService>();
            services.AddScoped<IGetItemsService, GetItemsService>();
            services.AddScoped<ICreateItemService, CreateItemService>();
            services.AddScoped<IUpdateItemService, UpdateItemService>();
            services.AddScoped<IDeleteItemService, DeleteItemService>();
            //log
            Console.WriteLine($"***{nameof(ApplicationConfiguration)} done.");
            return services;
        }
    }
}