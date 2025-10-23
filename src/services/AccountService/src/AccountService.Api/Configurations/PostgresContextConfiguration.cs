
using System;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Configurations;
using AccountService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Api.Configurations
{
    public static class PostgresContextConfiguration
    {
        public static IServiceCollection AddPostgreSqlContextConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring PostgreSQL..");
                var settings = configuration.GetSection(nameof(PostgresSettings))
                    .Get<PostgresSettings>() ??
                    throw new Exception();
                services.AddDbContext<PostgresDbContext>(config =>
                {
                    config.UseNpgsql(settings.ConnectionString);
                });
                _logger.LogInformation("PostgreSQL configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure PostgreSQL settings!");
                throw new InvalidOperationException(nameof(Program));
            }
        }
    }
}
