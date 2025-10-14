
using System;
using System.Text;
using CatalogManagementService.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CatalogManagementService.Api.Configurations
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            //log
            Console.WriteLine($"***{nameof(JwtConfiguration)} is being configured.");
            var settings = configuration.GetSection(nameof(JwtSettings))
                .Get<JwtSettings>() ??
                throw new NullReferenceException();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(settings.Key)),
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                    };
                });
            return services;
        }
    }
}