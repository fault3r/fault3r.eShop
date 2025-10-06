using System;
using CatalogManagementService.Application.Mediator.Handlers.Commands;
using CatalogManagementService.Application.Mediator.Handlers.Queries;

namespace CatalogManagementService.Api.Configurations
{
    public static class MediatrConfiguration
    {
        public static IServiceCollection AddMediatrConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(GetItemsQueryHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(GetItemQueryHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(CreateItemCommandHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(UpdateItemCommandHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(DeleteItemCommandHandler).Assembly);
            });
            return services;
        }  
    }
}