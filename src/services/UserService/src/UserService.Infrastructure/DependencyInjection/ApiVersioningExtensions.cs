
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddApiVersioning(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(AppSettings))
            .Get<AppSettings>()
        ?? throw new MissingAppSettingsException();

        float version = settings.ApiVersion;
        version = (float)Math.Round(version, 1);

        if (version < 1.0f)
            throw new InvalidApiVersionException();

        int major = (int)version;
        int minor = (int)((version - major) * 10);

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(major, minor);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader(settings.VersionParameter),
                new HeaderApiVersionReader(settings.VersionHeader)
            );
        });

        return services;
    }
}