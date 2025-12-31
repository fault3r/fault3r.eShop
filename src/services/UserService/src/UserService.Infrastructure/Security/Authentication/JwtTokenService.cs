
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Security.Authentication;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Security.Authentication;

public sealed class JwtTokenService(IOptions<JwtSetting> options)
{
    private readonly JwtSetting _settings = options.Value;

    public string GenerateAccessToken(SessionData session)
    {
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new("sessionId", session.SessionId),
            new(ClaimTypes.Sid, session.UserId),
            new(ClaimTypes.NameIdentifier, session.Email),
            new(ClaimTypes.Name, session.FullName),
            new(ClaimTypes.Role, session.Role),
            new(ClaimTypes.StateOrProvince, session.Status),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = now.AddMinutes(_settings.AccessTokenLifetimeMinutes);

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

    public static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        
        return Convert.ToBase64String(bytes);
    }
}