
using System;
using System.Security.Claims;
using UserService.Application.Security.Authentication;

namespace UserService.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string sessionId);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateAccessToken(string? token);
}
