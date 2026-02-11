
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Infrastructure.Exceptions.DependencyInjection;
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
            string connectionString = settings.ConnectionString;
            return ConnectionMultiplexer.Connect(connectionString);
        });

        services.AddSingleton<IDatabase>(sp =>
        {
            var connection = sp.GetRequiredService<IConnectionMultiplexer>();
            return connection.GetDatabase();
        });
        
        return services;
    }
}
