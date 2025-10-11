
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace CatalogManagementService.Api.Configurations
{
    public static class VersioningConfiguration
    {
        public static IServiceCollection AddVersioningConfiguration(this IServiceCollection services,
            decimal defaultVersion)
        {
            string[] version = defaultVersion.ToString().Split('.');
            int major = Convert.ToInt16(version[0]);
            int minor = Convert.ToInt16(version[1]);
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(major, minor);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            return services;
        }
        
    }
}