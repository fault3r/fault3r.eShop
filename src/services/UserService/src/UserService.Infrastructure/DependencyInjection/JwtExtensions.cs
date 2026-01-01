
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSetting>(
            configuration.GetSection(nameof(JwtSetting))
        );

        var settings = configuration
            .GetSection(nameof(JwtSetting))
            .Get<JwtSetting>()
                ?? throw new MissingJwtSettingException();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = settings.Issuer,
            ValidAudience = settings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(settings.SigningKey)
            ),
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton<TokenValidationParameters>(tokenValidationParameters);

        services.AddAuthentication(config =>
        {
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = tokenValidationParameters;
            });

        services.AddAuthorization(config =>
        {
            config.AddPolicy("requiredUser", policy => policy.RequireRole("User"));
        });

        services.AddSingleton<ITokenService, JwtTokenService>();

        return services;
    }
}
