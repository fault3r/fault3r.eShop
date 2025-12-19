
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Polly;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class PostgresExtensions
{
    public static IServiceCollection AddPostgresDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var setting = configuration
            .GetSection(nameof(PostgresSetting))
            .Get<PostgresSetting>()
                ?? throw new MissingPostgresSettingException();

        var connectionString = setting.ToConnectionString();

        services.AddDbContext<EfDbContext>(config =>
        {
            config.UseNpgsql(connectionString, config =>
            {
                config.MigrationsAssembly(
                    typeof(EfDbContext).Assembly.FullName);

                config.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(2),
                    errorCodesToAdd: null
                );
            });
        });

        TestConnection(connectionString);

        return services;
    }

    private static void TestConnection(string connectionString)
    {
        var retryPolicy = Policy
            .Handle<NpgsqlException>()
            .WaitAndRetry(
                retryCount: 3,
                sleepDurationProvider:
                    attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
            );

        try
        {
            retryPolicy.Execute(() =>
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
            });
        }
        catch { throw new PostgresConnectionException(); }
    }
}
