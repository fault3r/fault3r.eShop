
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.Aggregates;

public class UserAggregateTests
{
    [Fact]
    public void Create_WithValidInputs_SetPropertiesAndRaiseEvent()
    {
        var id = Identity.From(Guid.NewGuid());
        var email = Email.Parse("test@example.com");
        var hash = PasswordHash.Parse("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        var salt = PasswordSalt.Parse("salt");
        var fullname = FullName.From("John", "Doe");
        var role = Role.User;
        var status = Status.Active;

        var user = User.Create(id, email, hash, salt, fullname, role, status);

        Assert.Equal(id, user.Id);
        Assert.Equal(email, user.Email);
        Assert.Equal(hash, user.PasswordHash);
        Assert.Equal(salt, user.PasswordSalt);
        Assert.Equal(fullname, user.FullName);
        Assert.Equal(role, user.Role);
        Assert.Equal(status, user.Status);
    }
}
