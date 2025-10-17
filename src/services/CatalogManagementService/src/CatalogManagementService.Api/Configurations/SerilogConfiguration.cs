
using System;
using Serilog;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Services;

namespace CatalogManagementService.Api.Configurations
{
    public static class SerilogConfiguration
    {
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services,
            string filename)
        {
            Log.Logger = new LoggerConfiguration()
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
            return services;
        }
    }
}
