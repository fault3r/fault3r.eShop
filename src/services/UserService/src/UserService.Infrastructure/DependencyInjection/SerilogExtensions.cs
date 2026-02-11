
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilogLogging(
        this IHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            var settings = context.Configuration
                .GetSection($"Logging:{nameof(SerilogSettings)}")
                .Get<SerilogSettings>()
                    ?? throw new MissingSerilogSettingsException();

            config
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .MinimumLevel.Override("System", LogEventLevel.Verbose)
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    path: settings.Filename,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} {CorrelationId} {Message:lj} {NewLine} {Exception}"
                );
        })
            .ConfigureServices((_, services) =>
            {
                services.AddLogging(config =>
                {
                    config.AddSerilog();
                });
            });

        return host;
    }
}
