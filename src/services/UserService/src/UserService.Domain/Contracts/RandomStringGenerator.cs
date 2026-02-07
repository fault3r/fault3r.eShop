
using System;

namespace UserService.Domain.Contracts;

public static class RandomStringGenerator
{
    private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly int Lenght = Characters.Length;
    private static readonly Random random = new();

    public static string Generate(int length = 16)
    {
        var buffer = new char[length];

        for (int i = 0; i < length; i++)
            buffer[i] = Characters[random.Next(Lenght)];

        return string.Concat(buffer);
    }
}
