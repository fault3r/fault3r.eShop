
using System;

namespace JIT;


class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("\napp started.");

        var user = User.Create("fault3r@mail.com");

        Console.WriteLine($"Email: {user.Email}");
        foreach (var item in user.Events)
            Console.WriteLine($"{item.Id} : {item.Message}");

        Console.WriteLine("\neverything is ended.");
    }
}



#region  Domain

public interface IValueObject { }

public abstract class ValueObject : IValueObject { }

public sealed class EmailValueObject(
    string email
) : ValueObject
{
    public string Email { get; init; } = email;

    public override string ToString() => Email;
}

public record DomainEvent(Guid Id, string Message);

public class Entity
{
    public Guid Id { get; init; }
}

public abstract class AggregateRoot : Entity
{
    public List<DomainEvent> Events = [];
}

public class User(
    EmailValueObject email
) : AggregateRoot
{
    public EmailValueObject Email { get; init; } = email;

    public static User Create(string email)
    {
        var user = new User(new EmailValueObject(email));
        user.Events.Add(new DomainEvent(Guid.NewGuid(), "User Created!"));
        return user;
    }

}

#endregion
