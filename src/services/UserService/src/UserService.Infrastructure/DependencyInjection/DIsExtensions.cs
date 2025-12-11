
using System;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Interfaces;
using UserService.Domain.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.Services;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Correlation;
using UserService.Infrastructure.Messaging.Outbox;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Security;
using UserService.Infrastructure.UnitOfWork;

namespace UserService.Infrastructure.DependencyInjection;

public static class DIsExtensions
{
    public static IServiceCollection AddDIs(this IServiceCollection services)
    {
        services.AddScoped<IUserDomainService, UserDomainService>();

        services.AddScoped<IUserRepository, EfUserRepository>();

        services.AddScoped<IOutbox, EfOutbox>();

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddSingleton<ICorrelationContext, CorrelationContext>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        return services;
    }
}
