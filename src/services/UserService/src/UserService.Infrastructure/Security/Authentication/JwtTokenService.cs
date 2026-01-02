
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class JwtTokenService(
    TokenValidationParameters tokenValidationParameters,
    IOptions<JwtSetting> options)
        : ITokenService
{
    private readonly TokenValidationParameters _tokenValidation = tokenValidationParameters.Clone();
    private readonly JwtSetting _settings = options.Value;

    public string GenerateAccessToken(string sessionId, string userId)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, sessionId), 
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = now.AddMinutes(_settings.TokenLifetimeMinutes);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: credential
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ReadExpiredToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler();

        try
        {
            return handler.ValidateToken(token, _tokenValidation, out _);

        }
        catch { return null; }
    }
}