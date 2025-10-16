
using System;
using Serilog;

namespace CatalogManagementService.Api.Configurations
{
    public static class SerilogConfiguration
    {
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services,
            string logFile)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(logFile)
               .CreateLogger();
            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddSerilog(dispose: true);
            });
            return services;
        }
    }
}
