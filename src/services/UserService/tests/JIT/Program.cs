
using System;
using System.Security.Cryptography;

namespace JIT;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.\n");

        var writer = new ConsoleWriter();

        await writer.WriteAsync("fault3r",0,0);

        Console.WriteLine("\neverything is ended.");
    }
}

public sealed class ConsoleWriter
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public async Task WriteAsync(string quote, int xPos, int yPos)
    {
        int d = 100;

        byte[] buffer = new byte[d];
        RandomNumberGenerator.Fill(buffer);

        foreach(byte b in buffer)
        {
            char c = Chars[b % Chars.Length];

            Console.Write(c);

            await Task.Delay(b);
        }
    }
}