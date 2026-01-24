
using System;
using System.Security.Claims;

namespace UserService.Domain.Security.Authentication;

public interface ITokenService
{
    string GenerateAccessTokenAsync(string sessionId,string userId);
    ClaimsPrincipal? ReadAccessTokenClaimsAsync(string token);

    string GenerateRefreshToken();
    string HashRefreshToken(string raw);
    bool VerifyRefreshToken(string raw, string hash);
}
