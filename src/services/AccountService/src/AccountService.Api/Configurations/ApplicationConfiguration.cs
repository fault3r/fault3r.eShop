
using System;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Repositories;

namespace AccountService.Api.Configurations
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            using var provider = services.BuildServiceProvider();
            var _logger = provider.GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring Application..");
                services.AddScoped<IRepository, PostgresRepository>();
                _logger.LogInformation("Application configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure Application settings!");
                throw new InvalidOperationException(nameof(Program));
            }
        }
    }
}