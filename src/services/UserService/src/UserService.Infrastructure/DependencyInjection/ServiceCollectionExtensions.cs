
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Correlation;
using UserService.Infrastructure.Security;

namespace UserService.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ICorrelationContext, CorrelationContext>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        
        return services;
    }
}
