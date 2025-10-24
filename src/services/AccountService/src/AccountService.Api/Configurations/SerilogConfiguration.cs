
using System;
using Serilog;
using Serilog.Events;
using AccountService.Infrastructure.Services;
using AccountService.Application.Interfaces.Services;

namespace AccountService.Api.Configurations
{
    public static class SerilogConfiguration
    {
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services,
            string filename)
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
            services.AddSingleton(
                typeof(ILoggerService<>),
                typeof(SerilogLoggerService<>));
            using (var provider = services.BuildServiceProvider())
            {
                var _logger = provider.GetRequiredService<ILoggerService<Program>>();
                _logger.LogInformation("Serilog configured successfully.");
            }
            return services;
        }
    }
}
