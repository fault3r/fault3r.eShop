
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Polly;
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

            services.AddDbContext<EfDbContext>(config =>
            {
                config.UseNpgsql(connectionString, config =>
                {
                    config.MigrationsAssembly(
                        typeof(EfDbContext).Assembly.FullName);

                    config.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(3),
                        errorCodesToAdd: null);
                });
            });

            var retryPolicy = Policy
                .Handle<NpgsqlException>()
                .WaitAndRetry(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, timespan, attempt, context) =>
                    {
                        string log = $"Retry {attempt} after {timespan.TotalSeconds}s due to: {exception.Message}";
                    });
            try
            {
                retryPolicy.Execute(() =>
                {
                    using var connection = new NpgsqlConnection(connectionString);
                    connection.Open();
                    connection.Close();
                });
            }
            catch { throw new PostgresConnectionException(); }
        });
    }
}
