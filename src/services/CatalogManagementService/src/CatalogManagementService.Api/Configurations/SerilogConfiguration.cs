
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Services;
using Serilog;

namespace CatalogManagementService.Api.Configurations
{
    public static class SerilogConfiguration
    {
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services,
            string logFile)
        {
            Log.Logger = new LoggerConfiguration()
               .WriteTo.File(logFile)
               .CreateLogger();
            services.AddLogging(options =>
            {
                options.AddSerilog(dispose: true);
            });
            services.AddSingleton(
                typeof(ILoggerService<>),
                typeof(SerilogLoggerService<>));
            return services;
        }
    }
}
