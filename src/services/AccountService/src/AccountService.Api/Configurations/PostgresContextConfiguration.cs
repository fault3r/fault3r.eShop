
using System;
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
            var settings = configuration.GetSection(nameof(PostgresSettings))
                .Get<PostgresSettings>() ??
                throw new Exception();
            services.AddDbContext<PostgresDbContext>(config =>
            {
                config.UseNpgsql(settings.ConnectionString);
            });
            return services;
        }
    }
}
