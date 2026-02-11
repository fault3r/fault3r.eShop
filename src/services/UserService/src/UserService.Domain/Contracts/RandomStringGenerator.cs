
using System;
using System.Security.Cryptography;

namespace UserService.Domain.Contracts;

public static class RandomStringGenerator
{
    public const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly int Length = Characters.Length;

    public static string GetString(int length = 8)
    {
        var buffer = new byte[length];
        RandomNumberGenerator.Fill(buffer);

        var generated = new char[length];
        for (int i = 0; i < length; i++)
            generated[i] = Characters[buffer[i] % Length];

        return new string(generated);
    }
}
