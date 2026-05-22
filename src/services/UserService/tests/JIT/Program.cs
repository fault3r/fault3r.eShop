
using System;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.");


        Console.WriteLine("everything is ended.");
    }
}

public enum GenderType
{
    Male = 1,
    Female = 0,
}

public sealed class Gender(GenderType type)
{
    public GenderType Value { get; init; } = type;
}

