
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User;

public class User : AggregateRoot<User, Identity>
{
    public User(Identity id) : base(id)
    {
        if(id is null)
            throw new MissingIdentityException();
    }
}
