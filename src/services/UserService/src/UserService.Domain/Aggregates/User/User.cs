
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User;

public class User : AggregateRoot<User, Identity>
{
    public User(Identity id) : base(id) { }
}
