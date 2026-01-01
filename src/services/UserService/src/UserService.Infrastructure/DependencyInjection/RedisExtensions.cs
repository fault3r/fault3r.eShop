
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserService.Application.Interfaces;
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
        services.Configure<RedisSetting>(
            configuration.GetSection(nameof(RedisSetting))
        );

        var settings = configuration
            .GetSection(nameof(RedisSetting))
            .Get<RedisSetting>()
                ?? throw new MissingRedisSettingException();

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            return ConnectionMultiplexer.Connect(
                settings.ToConnectionString());
        });

        services.AddSingleton<ISessionService, RedisSessionService>();

        return services;
    }
}
