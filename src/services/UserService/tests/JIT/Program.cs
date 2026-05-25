
using System;

namespace JIT;


class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.");
        
        Console.WriteLine("everything is ended.");
    }
}



#region  Domain

public interface IValueObject { }

public abstract class ValueObject : IValueObject { }

public class GenderValueObject(
    string email
) : ValueObject
{
    public string Email { get; init; } = email;
}



#endregion
