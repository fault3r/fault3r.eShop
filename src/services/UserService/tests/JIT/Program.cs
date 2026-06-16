
using System;
using System.Security.Cryptography;

namespace JIT;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.\n");

        var writer = new ConsoleWriter();

        await writer.WriteAsync("fault3r");

        Console.WriteLine("\neverything is ended.");
    }
}

public sealed class ConsoleWriter
{
    private const string Dummy = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public async Task WriteAsync(string quote)
    {
        byte[] bytes = new byte[Dummy.Length];
        RandomNumberGenerator.
        for (int i = 0; i < 10; i++)
            x = new Random().Next(0, Dummy.Length);


    }
}