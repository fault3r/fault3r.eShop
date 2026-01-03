
using System;
using System.Security.Claims;

namespace UserService.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(string sessionId, string userId);
    Task<ClaimsPrincipal?> ReadClaimsAsync(string token);
}
