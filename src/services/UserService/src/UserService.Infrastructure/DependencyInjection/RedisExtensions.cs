
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Domain.Security.Authentication;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class RedisExtensions
{
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(RedisSettings))
            .Get<RedisSettings>()
                ?? throw new MissingRedisSettingsException();    

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            return ConnectionMultiplexer.Connect(
                settings.ToConnectionString());
        });

        services.AddSingleton<ISessionService>(provider =>
        {
            var connection = provider.GetRequiredService<IConnectionMultiplexer>();
            return new RedisSessionService(connection, settings);
        });

        return services;
    }
}
