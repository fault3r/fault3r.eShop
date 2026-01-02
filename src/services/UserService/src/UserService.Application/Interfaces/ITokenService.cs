
using System;
using System.Security.Claims;

namespace UserService.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(string sessionId, string userId);
    Task<ClaimsPrincipal?> ReadClaims(string token);
}
