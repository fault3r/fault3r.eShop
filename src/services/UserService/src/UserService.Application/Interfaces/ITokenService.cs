
using System;
using System.Security.Claims;

namespace UserService.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string sessionId);
    ClaimsPrincipal? ReadPrincipal(string token);
}
