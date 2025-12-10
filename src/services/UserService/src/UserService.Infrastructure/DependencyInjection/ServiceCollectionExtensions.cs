
using System;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Correlation;

namespace UserService.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ICorrelationContext, CorrelationContext>();
        return services;
    }
}
