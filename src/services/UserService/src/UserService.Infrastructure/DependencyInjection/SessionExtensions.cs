
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Application.CrossCutting;
using UserService.Domain.Security.Authentication;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class SessionExtensions
{
    public static IServiceCollection AddSession(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(SessionSettings))
            .Get<SessionSettings>()
        ?? throw new MissingSessionSettingsException();

        services.AddSingleton<ISessionService>(sp =>
        {
            var connection = sp.GetRequiredService<IDatabase>();
            var serializer = sp.GetRequiredService<IJsonSerializer>();
            return new RedisSessionService(connection, settings, serializer);
        });
        
        return services;
    }
}
