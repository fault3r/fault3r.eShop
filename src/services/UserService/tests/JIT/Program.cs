
using System;
using System.Security.Cryptography;

namespace JIT;

class Program
{
    public static async Task Main(string[] args)
    {
        string text = args[0] is null ? "" : args[0];
        var writer = new ConsoleWriter();
        await writer.WriteAsync(text);
    }
}

public sealed class ConsoleWriter
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private readonly Random random = new();

    public async Task WriteAsync(string text)
    {
        foreach (char c in text)
        {
            (int left, int top) = Console.GetCursorPosition();

            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(0, 255);
                char d = Chars[x % Chars.Length];

                Console.Write(d);
                Console.SetCursorPosition(left, top);

                await Task.Delay(random.Next(0,80));
            }

            Console.Write(c);
        }
    }
}