
using System;
using System.Text;
using CatalogManagementService.Application.Interfaces;
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
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring JsonWebToken..");
                var settings = configuration.GetSection(nameof(JwtSettings))
                    .Get<JwtSettings>() ??
                    throw new Exception();
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
                _logger.LogInformation("JsonWebToken configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure JsonWebToken settings!");
                throw new InvalidOperationException(nameof(Program));     
            }
        }
    }
}