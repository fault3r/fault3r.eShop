
using System;
using System.Net.Mail;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class EmailTests
{

    [Fact]
    public void Constructor_WithNullMailAddress_ShouldThrowException()
    {
        Assert.Throws<EmptyEmailAddressException>(() => new Email((MailAddress)null!));
    }

    [Fact]
    public void Constructor_WithValidMailAddress_ShouldNormalizeAndSetValue()
    {
        var mailAddress = new MailAddress("  Test@Example.Com  ");

        var email = new Email(mailAddress);

        Assert.Equal("test@example.com", email.ToString());
    }

    [Fact]
    public void Constructor_WithEmptyString_ShouldThrowException()
    {
        Assert.Throws<EmptyEmailAddressException>(() => new Email(""));
    }

    [Fact]
    public void Constructor_WithInvalidString_ShouldThrowException()
    {
        Assert.Throws<InvalidEmailAddressException>(() => new Email("invalid-email"));
    }

    [Fact]
    public void Constructor_WithValidString_ShouldNormalizeAndSetValue()
    {
        var email = new Email("  Test@Example.Com  ");

        Assert.Equal("test@example.com", email.ToString());
    }
}