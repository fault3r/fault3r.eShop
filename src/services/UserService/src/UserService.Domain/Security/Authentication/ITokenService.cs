
using System;
using System.Security.Claims;

namespace UserService.Domain.Security.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(string sessionId, string userId);
    ClaimsPrincipal? ReadAccessTokenClaims(string token);

    string GenerateRefreshToken();
    string ComputeRefreshTokenHash(string raw);
    bool VerifyRefreshToken(string raw, string hash);
}
