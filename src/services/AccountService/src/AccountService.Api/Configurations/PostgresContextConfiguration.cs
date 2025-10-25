
using System;
using AccountService.Api.Exceptions;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Configurations;
using AccountService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Configurations
{
    public static class PostgresContextConfiguration
    {
        public static IServiceCollection AddPostgresContextConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            using var provider = services.BuildServiceProvider();
            var _logger = provider.GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring PostgreSQL..");
                var settings = configuration.GetSection(nameof(PostgresSettings))
                    .Get<PostgresSettings>() ??
                    throw new Exception();
                string connectionString =
                    $"Host={settings.Host};" +
                    $"Port={settings.Port};" +
                    $"Username={settings.Username};" +
                    $"Password={settings.Password};" +
                    $"Database={settings.Database};";
                services.AddDbContext<PostgresDbContext>(config =>
                {
                    config.UseNpgsql(connectionString);
                });
                _logger.LogInformation("PostgreSQL configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure PostgreSQL settings!");
                throw new InvalidConfigurationException();
            }
        }
    }
}
