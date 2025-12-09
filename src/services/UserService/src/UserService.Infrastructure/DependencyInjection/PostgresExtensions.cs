
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
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(EfDbContext).Assembly.FullName);

                    // Retry policy
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,                  // how many times to retry
                        maxRetryDelay: TimeSpan.FromSeconds(10), // max delay between retries
                        errorCodesToAdd: null              // you can add specific Postgres error codes if needed
                    );
                }));

        });
    }
}
