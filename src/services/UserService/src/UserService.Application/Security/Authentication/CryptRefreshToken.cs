
using System;
using System.Security.Cryptography;

namespace UserService.Application.Security.Authentication;

public static class CryptRefreshToken
{
    public static string GenerateToken(int length = 64)
    {
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return Convert.ToBase64String(bytes);
    }

    public static string ToBCryptHash(string raw)
    {
        return BCrypt.Net.BCrypt.HashPassword(raw);
    }

    public static string VerifyBCryptToken(string raw)
    {
        return BCrypt.Net.BCrypt.HashPassword(raw);
    }
}
