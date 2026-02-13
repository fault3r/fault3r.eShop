
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class PostgresExtensions
{
    public static IServiceCollection AddEfPostgresDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(PostgresSettings))
            .Get<PostgresSettings>()
        ?? throw new MissingPostgresSettingsException();

        var connectionString = settings.ToConnectionString();

        services.AddDbContext<EfPostgresDbContext>(config =>
        {
            config.UseNpgsql(connectionString, config =>
            {
                config.MigrationsAssembly(
                    typeof(EfPostgresDbContext).Assembly.FullName);

                config.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(2),
                    errorCodesToAdd: null
                );
            });
        });

        return services;
    }
}
