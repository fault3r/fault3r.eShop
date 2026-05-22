
using System;

namespace JIT;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.");

        var me = new Fault3r();
        Console.WriteLine($"{me.Name}, {me.Gender.Value}");

        Console.WriteLine("everything is ended.");
    }
}

public enum GenderType
{
    Male = 1,
    Female = 0,
}

public class Gender(GenderType type)
{
    public GenderType Value { get; init; } = type;

    public static readonly Gender Male = new(GenderType.Male);
}

public class Human
{
    public Gender Gender { get; set; }
     = Gender.Male;
}

public sealed class Fault3r : Human
{
    public string Name { get; set; }
        = "Hamed Damavandi";
}