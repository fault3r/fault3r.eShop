
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using UserService.Infrastructure.Exceptions.Logging;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilogConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, config) =>
        {
            var settings = context.Configuration
                .GetSection(nameof(SerilogSettings))
                .Get<SerilogSettings>()
                    ?? throw new MissingLoggerException();

            config.MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                  .MinimumLevel.Override("System", LogEventLevel.Fatal)
                  .MinimumLevel.Debug()
                  .WriteTo.File(settings.Filename);
        });
    }
}
