
using System;

namespace JIT;


class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("app started.\n");

        var user = UserFactory.CreateUser("test@mail.com");

        user.ChangeEmail("fault3r@mail.com");

        Console.WriteLine($"Email: {user.Email}");
        
        foreach (var item in user.Events)
            Console.WriteLine(item.Message);

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
    public string Value { get; set; } = email;

    public override string ToString() => Value;
}

public record DomainEvent(string Message);

public class Entity(
    Guid id
)
{
    public Guid Id { get; init; } = id;
}

public abstract class AggregateRoot(Guid id) : Entity(id)
{
    private readonly List<DomainEvent> events = [];

    public IReadOnlyCollection<DomainEvent> Events => [..events];

    public void RaiseEvent(DomainEvent @event) => events.Add(@event);

    protected void ClearEvents() => events.Clear();
}

public sealed class User(
    EmailValueObject email
) : AggregateRoot(Guid.NewGuid())
{
    public EmailValueObject Email { get; private set; } = email;

    public void ChangeEmail(string email)
    {
        Email.Value = email;
        RaiseEvent(new DomainEvent($"Email {Id} changed to {email}."));        
    }
}

public static class UserFactory
{
    public static User CreateUser(string email)
    {
        var user = new User(new EmailValueObject(email));
        user.RaiseEvent(new DomainEvent($"User {user.Id} Created!"));
        return user;
    }
}

#endregion
