
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Application.UseCases.CreateItem;
using CatalogService.Application.UseCases.DeleteItem;
using CatalogService.Application.UseCases.GetItem;
using CatalogService.Application.UseCases.GetItems;
using CatalogService.Application.UseCases.UpdateItem;

namespace CatalogService.Api.Configurations
{
    public static class MediatrConfiguration
    {
        public static IServiceCollection AddMediatrConfiguration(this IServiceCollection services)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring MediatR..");
                services.AddMediatR(options =>
                {
                    options.RegisterServicesFromAssembly(typeof(GetItemsQueryHandler).Assembly);
                    options.RegisterServicesFromAssembly(typeof(GetItemQueryHandler).Assembly);
                    options.RegisterServicesFromAssembly(typeof(CreateItemCommandHandler).Assembly);
                    options.RegisterServicesFromAssembly(typeof(UpdateItemCommandHandler).Assembly);
                    options.RegisterServicesFromAssembly(typeof(DeleteItemCommandHandler).Assembly);
                });
                _logger.LogInformation("MediatR configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure MediatR settings!");
                throw new InvalidOperationException(nameof(Program));   
            }
        }
    }
}