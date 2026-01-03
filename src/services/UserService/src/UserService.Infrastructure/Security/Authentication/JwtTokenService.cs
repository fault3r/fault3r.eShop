
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class JwtTokenService(
    TokenValidationParameters tokenValidationParameters,
    JwtSettings settings
) : ITokenService
{
    private readonly TokenValidationParameters _tokenValidation = tokenValidationParameters;
    private readonly JwtSettings _settings = settings;

    public Task<string> GenerateAccessTokenAsync(
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

        var jwt = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: credential
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Task.FromResult(token);
    }

    public Task<ClaimsPrincipal?> ReadClaimsAsync(string token)
    {
        ArgumentNullException.ThrowIfNull(token);

        var handler = new JwtSecurityTokenHandler
        {
            MapInboundClaims = false
        };

        try
        {
            var claims = handler.ValidateToken(token, _tokenValidation, out _);
            
            return Task.FromResult<ClaimsPrincipal?>(claims);
        }
        catch { return Task.FromResult<ClaimsPrincipal?>(null); }
    }
}
