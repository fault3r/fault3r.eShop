
using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Domain.Security.Authentication;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class SessionExtensions
{
    public static IServiceCollection AddApplicationSession(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(SessionSettings))
            .Get<SessionSettings>()
                ?? throw new MissingSessionSettingsException();    

        services.AddSingleton<ISessionService>(provider =>
        {
            var connection = provider.GetRequiredService<IConnectionMultiplexer>();
            return new RedisSessionService(connection, settings);
        });

        return services;
    }
}
