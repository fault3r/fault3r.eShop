
using System;
using AccountService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace AccountService.Api.Configurations
{
    public static class VersioningConfiguration
    {
        public static IServiceCollection AddVersioningConfiguration(this IServiceCollection services,
            decimal defaultVersion)
        {
            using var provider = services.BuildServiceProvider();
            var _logger = provider.GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring Versioning..");
                string[] version = defaultVersion.ToString().Split('.');
                int major = Convert.ToInt16(version[0]);
                int minor = Convert.ToInt16(version[1]);
                services.AddApiVersioning(config =>
                {
                    config.DefaultApiVersion = new ApiVersion(major, minor);
                    config.ReportApiVersions = true;
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    config.ApiVersionReader = new UrlSegmentApiVersionReader();
                });
                _logger.LogInformation("Versioning configured successfully.");
                return services;
            }
            catch
            {
                 _logger.LogError("failed to configure Versioning settings!");
                throw new InvalidOperationException(nameof(Program));  
            }
        }
    }
}