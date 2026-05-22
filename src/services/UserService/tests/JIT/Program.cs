
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

public class Gender(GenderType type)
{
    public GenderType Value { get; init; } = type;
}

public class Human
{
    public Gender Gender { get; set; } = gender;
}

public sealed class Fault3r :  Human
{
    public const string Name = "Hamed Damavandi";

    public Fault3r(Gender gender) : base(gender)
    {
        
    }
}