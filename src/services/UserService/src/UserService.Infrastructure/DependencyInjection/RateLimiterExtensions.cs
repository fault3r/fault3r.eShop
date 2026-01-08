
using System;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(RateLimitingSettings))
            .Get<RateLimitingSettings>()
                ?? throw new MissingRateLimitingSettings();

        services.AddRateLimiter(config =>
        {
            config.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings.PermitLimit,
                        Window = TimeSpan.FromMinutes(settings.Window),
                        QueueLimit = settings.QueueLimit,
                        QueueProcessingOrder = settings.IsOldestFirst 
                            ? QueueProcessingOrder.OldestFirst
                            : QueueProcessingOrder.NewestFirst
                    }
                )
            );

            config.AddFixedWindowLimiter("RefAuthRateLimit", options =>
            {
                options.PermitLimit = settings.RefAuthRateLimit.PermitLimit;
                options.Window = TimeSpan.FromMinutes(settings.RefAuthRateLimit.Window);
            });
        });

        return services;
    }
}
