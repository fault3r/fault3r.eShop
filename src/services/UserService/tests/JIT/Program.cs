
using System;

namespace JIT;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.\n");



        Console.WriteLine("\neverything is ended.");
    }
}

public sealed class Writer
{
    private const string Characters = "abcdefghi";

    public async Task StartWrite(string quote)
    {
        int x = new Random().Next(0, Characters.Length);


    }
}