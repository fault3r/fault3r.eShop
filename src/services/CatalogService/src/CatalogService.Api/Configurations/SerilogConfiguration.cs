
using System;
using Serilog;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Services;
using Serilog.Events;

namespace CatalogService.Api.Configurations
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
            services.AddLogging(options =>
            {
                options.AddSerilog(dispose: true);
            });
            services.AddSingleton(
                typeof(ILoggerService<>),
                typeof(SerilogLoggerService<>));
             var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            _logger.LogInformation("Serilog configured successfully.");
            return services;
        }
    }
}
