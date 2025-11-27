using System;
using System.Net.Mail;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Constructor_WithValidMailAddress_ShouldSetValue()
    {
        var mailAddress = new MailAddress("test@example.com");

        var email = new Email(mailAddress);

        Assert.Equal(mailAddress, email.Value);
        Assert.Equal("test@example.com", email.ToString());
    }

    [Fact]
    public void Constructor_WithNullMailAddress_ShouldThrowEmptyEmailAddressException()
    {
        Assert.Throws<EmptyEmailAddressException>(() => new Email((MailAddress)null!));
    }

    [Fact]
    public void Constructor_WithValidString_ShouldNormalizeAndSetValue()
    {
        var email = new Email("  Test@Example.Com  ");

        Assert.Equal("test@example.com", email.Value.Address);
        Assert.Equal("test@example.com", email.ToString());
    }

    [Fact]
    public void Constructor_WithEmptyString_ShouldThrowEmptyEmailAddressException()
    {
        Assert.Throws<EmptyEmailAddressException>(() => new Email(""));
    }

    [Fact]
    public void Constructor_WithInvalidString_ShouldThrowInvalidEmailAddressException()
    {
        Assert.Throws<InvalidEmailAddressException>(() => new Email("invalid-email"));
    }

    [Fact]
    public void From_WithValidMailAddress_ShouldReturnEmail()
    {
        var mailAddress = new MailAddress("user@example.com");

        var email = Email.From(mailAddress);

        Assert.Equal("user@example.com", email.Value.Address);
    }

    [Fact]
    public void Parse_WithValidString_ShouldReturnEmail()
    {
        var email = Email.Parse("parse@example.com");

        Assert.Equal("parse@example.com", email.Value.Address);
    }

    [Fact]
    public void Equality_ShouldBeBasedOnValue()
    {
        var email1 = new Email("same@example.com");
        var email2 = new Email("same@example.com");
        var email3 = new Email("different@example.com");

        Assert.Equal(email1, email2);
        Assert.NotEqual(email1, email3);
    }

    [Fact]
    public void ToString_ShouldReturnNormalizedAddress()
    {
        var email = new Email("MixedCase@Example.Com");

        Assert.Equal("mixedcase@example.com", email.ToString());
    }
}