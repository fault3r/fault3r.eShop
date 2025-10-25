
using System;
using System.Text;
using AccountService.Api.Exceptions;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AccountService.Api.Configurations
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var provider = services.BuildServiceProvider();
            var _logger = provider.GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring JsonWebToken..");
                var settings = configuration.GetSection(nameof(JwtSettings))
                    .Get<JwtSettings>() ??
                    throw new Exception();
                services.AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(config =>
                    {
                        config.TokenValidationParameters = new TokenValidationParameters
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
                throw new InvalidConfigurationException();     
            }
        }
    }
}