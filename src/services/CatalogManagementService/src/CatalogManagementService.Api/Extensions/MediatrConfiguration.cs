using System;
using CatalogManagementService.Application.MediatR.Handlers.Commands;
using CatalogManagementService.Application.MediatR.Handlers.Queries;

namespace CatalogManagementService.Api.Extensions
{
    public static class MediatrConfiguration
    {
        public static IServiceCollection AddMediatrConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(GetAllQueryHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(GetByIdQueryHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(CreateCommandHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(UpdateCommandHandler).Assembly);
                options.RegisterServicesFromAssembly(typeof(DeleteCommandHandler).Assembly);
            });
            return services;
        }  
    }
}