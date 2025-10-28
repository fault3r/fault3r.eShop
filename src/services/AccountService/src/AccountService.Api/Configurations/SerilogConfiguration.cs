
using System;
using Serilog;
using Serilog.Events;
using AccountService.Api.Exceptions;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Services;

namespace AccountService.Api.Configurations
{
    public static class SerilogConfiguration
    {
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services,
            string filename)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                    .MinimumLevel.Override("System", LogEventLevel.Fatal)
                    .MinimumLevel.Debug()
                    .WriteTo.File(filename)
                    .CreateLogger();
                services.AddLogging(config =>
                {
                    config.AddSerilog(dispose: true);
                });
                services.AddScoped(typeof(ILoggerService<>), typeof(SerilogLoggerService<>));
                return services;
            }
            catch
            {
                throw new InvalidConfigurationException();
            }
        }
    }
}
