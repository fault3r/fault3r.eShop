
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.Aggregates;

public class UserAggregateTests
{
    private static readonly Identity id = Identity.From(Guid.NewGuid());
    private static readonly Email email = Email.Parse("test@example.com");
    private static readonly PasswordHash hash = PasswordHash.Parse("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
    private static readonly PasswordSalt salt = PasswordSalt.Parse("salt");
    private static readonly FullName fullname = FullName.From("Hamed", "Damaavandi");
    private static readonly Role role = Role.User;
    private static readonly Status status = Status.Active;

    [Fact]
    public void Create_WithValidInputs_SetPropertiesAndRaiseEvent()
    {
        var user = User.Create(id, email, hash, salt, fullname, role, status);
        var @event = Assert.IsType<UserRegisteredEvent>(user.Events.First());

        Assert.Equal(id, user.Id);
        Assert.Equal(email, user.Email);
        Assert.Equal(hash, user.PasswordHash);
        Assert.Equal(salt, user.PasswordSalt);
        Assert.Equal(fullname, user.FullName);
        Assert.Equal(role, user.Role);
        Assert.Equal(status, user.Status);

        Assert.NotEqual(Guid.Empty, @event.EventId);
        Assert.NotEqual(DateTimeOffset.MinValue, @event.OccurredOn);
        Assert.Equal(id, @event.UserId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(fullname, @event.FullName);
    }


    [Fact]
    public void ChangePassword_WithNewPassword_ChangePasswordAndRaiseEvent()
    {
        var user = User.Create(id, email, hash, salt, fullname, role, status);
        var newHash = PasswordHash.Parse("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
        user.ClearEvents();

        user.ChangePassword(newHash);
        var @event = Assert.IsType<UserPasswordChangedEvent>(user.Events.First());

        Assert.Equal(newHash, user.PasswordHash);

        Assert.NotEqual(Guid.Empty, @event.EventId);
        Assert.NotEqual(DateTimeOffset.MinValue, @event.OccurredOn);
        Assert.Equal(id, @event.UserId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(fullname, @event.FullName);
    }

    [Fact]
    public void ChangeFullName_WithNewFullName_ChangeFullNameAndRaiseEvent()
    {
        var user = User.Create(id, email, hash, salt, fullname, role, status);
        var newName = FullName.From("Ali", "Damaavandi");
        user.ClearEvents();

        user.ChangeFullName(newName);
        var @event = Assert.IsType<UserFullNameChangedEvent>(user.Events.First());

        Assert.Equal(newName, user.FullName);

        Assert.NotEqual(Guid.Empty, @event.EventId);
        Assert.NotEqual(DateTimeOffset.MinValue, @event.OccurredOn);
        Assert.Equal(id, @event.UserId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(user.FullName, @event.NewFullName);
    }

    [Fact]
    public void PromoteToAdmin_ChangeRoleToAdminAndRaiseEvent()
    {
        var user = User.Create(id, email, hash, salt, fullname, role, status);
        user.ClearEvents();

        user.PromoteToAdmin();
        var @event = Assert.IsType<UserRoleChangedEvent>(user.Events.First());

        Assert.NotEqual(Guid.Empty, @event.EventId);
        Assert.NotEqual(DateTimeOffset.MinValue, @event.OccurredOn);
        Assert.Equal(id, @event.UserId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(user.Role, @event.NewRole);
    }

    [Fact]
    public void DemoteToUser_ChangeRoleToUserAndRaiseEvent()
    {
        var user = User.Create(id, email, hash, salt, fullname, role, status);
        user.ClearEvents();

        user.DemoteToUser();
        var @event = Assert.IsType<UserRoleChangedEvent>(user.Events.First());

        Assert.NotEqual(Guid.Empty, @event.EventId);
        Assert.NotEqual(DateTimeOffset.MinValue, @event.OccurredOn);
        Assert.Equal(id, @event.UserId);
        Assert.Equal(email, @event.Email);
        Assert.Equal(user.Role, @event.NewRole);        
    }
}
