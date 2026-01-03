
using System;
using System.Security.Cryptography;

namespace UserService.Application.Security.Authentication;

public static class CryptRefreshToken
{
    public static string Generate(int length = 64)
    {
        var bytes = new byte[length];

        RandomNumberGenerator.Fill(bytes);

        return Convert.ToBase64String(bytes);
    }

    public static string Hash(string raw)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(raw);

        return BCrypt.Net.BCrypt.HashPassword(raw);
    }

    public static bool Verify(string raw, string hash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(raw);
        ArgumentException.ThrowIfNullOrWhiteSpace(hash);

        return BCrypt.Net.BCrypt.Verify(raw, hash);
    }
}
