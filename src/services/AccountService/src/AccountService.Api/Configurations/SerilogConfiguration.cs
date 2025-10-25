
using System;
using Serilog;
using Serilog.Events;
using AccountService.Api.Exceptions;

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
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .MinimumLevel.Override("System", LogEventLevel.Error)
                    .MinimumLevel.Debug()
                    .WriteTo.File(filename)
                    .CreateLogger();
                services.AddLogging(config =>
                {
                    config.AddSerilog(dispose: true);
                });
                Log.Information("tttttttttttttttttttttttttttttttttttttt");
                return services;
            }
            catch
            {
                throw new InvalidConfigurationException();
            }
        }
    }
}
