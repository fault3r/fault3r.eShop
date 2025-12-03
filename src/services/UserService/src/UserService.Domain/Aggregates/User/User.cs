
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Aggregates.User.Events;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.Exceptions.ValueObjects.Role;
using UserService.Domain.Exceptions.ValueObjects.Status;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User;

public class User : AggregateRoot<User, Identity>
{
    public Email Email { get;  private set; }
    public string PasswordHash { get; private set; }
    public FullName FullName { get; private set; }
    public Role Role { get; private set; }
    public Status Status { get; private set; }

    private User(Identity id, Email email, string passwordHash, FullName fullName, Role role, Status status)
        : base(id)
    {
        Email = email ?? throw new MissingEmailException();
        PasswordHash = passwordHash ?? throw new Exception(); // we are here
        FullName = fullName?? throw new MissingFullNameException();
        Role = role ??throw new MissingRoleException();
        Status = status ?? throw new MissingStatusException();
    }

    internal static User Create(
        Identity id, Email email, string passwordHash, FullName fullName, Role role, Status status)
    {
        var user = new User(id,email,passwordHash,fullName,role,status);
        user.RaiseEvent(new UserCreatedEvent(user.Id,user.Email));
        return user;
    }

    public void Behaviours(){}
}