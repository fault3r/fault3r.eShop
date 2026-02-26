
using System;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Persistence.Contexts;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(RabbitmqSettings))
            .Get<RabbitmqSettings>()
        ?? throw new MissingRabbitmqSettingsException();

        services.AddMassTransit(config =>
        {
            config.AddEntityFrameworkOutbox<EfPostgresDbContext>(cfg =>
            {
                cfg.QueryDelay = TimeSpan.FromSeconds(1);
                cfg.UsePostgres();
                cfg.UseBusOutbox(); 
            });

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(settings.HostName, c =>
                {
                    c.Username(settings.UserName);
                    c.Password(settings.Password);
                });

                // cfg.ReceiveEndpoint("user-events", e => { ... });
            });
        });

        return services;
    }
}
