
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.Domain.Contracts;
using UserService.Domain.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class JwtTokenService(
    TokenValidationParameters tokenValidationParameters,
    JwtSettings settings
) : ITokenService
{
    private readonly TokenValidationParameters _tokenValidation = tokenValidationParameters;
    private readonly JwtSettings _settings = settings;

    public string GenerateAccessToken(
        string sessionId,
        string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var now = DateTime.UtcNow;

        var expires = now.AddMinutes(_settings.TokenLifetimeMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, sessionId),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey));

        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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

    public ClaimsPrincipal? ReadAccessTokenClaims(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var handler = new JwtSecurityTokenHandler
        {
            MapInboundClaims = false,
        };

        try
        {
            var claims = handler.ValidateToken(token, _tokenValidation, out _);

            return claims;
        }
        catch { return null; }
    }

    public string GenerateRefreshToken()
    {
        return RandomStringGenerator.GetString(length: 50);
    }

    public string ComputeRefreshTokenHash(string raw)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(raw);

        return BCrypt.Net.BCrypt.HashPassword(raw);
    }

    public bool VerifyRefreshToken(string raw, string hash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(raw);
        ArgumentException.ThrowIfNullOrWhiteSpace(hash);

        return BCrypt.Net.BCrypt.Verify(raw, hash);
    }
}
