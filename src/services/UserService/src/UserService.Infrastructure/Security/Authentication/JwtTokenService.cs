
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
    IOptions<JwtSettings> options
) : ITokenService
{
    private readonly TokenValidationParameters _tokenValidation = tokenValidationParameters.Clone();
    private readonly JwtSettings _settings = options.Value;

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

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: credential
        );

        return Task.FromResult(
            new JwtSecurityTokenHandler().WriteToken(token));
    }

    public Task<ClaimsPrincipal?> ReadClaimsAsync(string token)
    {
        ArgumentNullException.ThrowIfNull(token);

        var handler = new JwtSecurityTokenHandler();

        try
        {
            var claims = handler.ValidateToken(token, _tokenValidation, out _);
            
            return Task.FromResult<ClaimsPrincipal?>(claims);
        }
        catch { return Task.FromResult<ClaimsPrincipal?>(null); }
    }
}
