
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Infrastructure.Exceptions.DependencyInjection.Postgres;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class PostgresExtensions
{
    public static IHostBuilder AddPostgresDbContext(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            var settings = context.Configuration
                .GetSection(nameof(PostgresSettings))
                .Get<PostgresSettings>()
                    ?? throw new MissingPostgresSettingsException();

            var connectionString = settings.ToConnectionString();

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidPostgresConnectionStringException();

            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                });
            });
        });
    }
}
