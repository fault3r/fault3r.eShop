
using System;
using System.Net.Mail;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class EmailTests
{

    [Fact]
    public void WithNullMailAddress_ThrowEmptyEmailAddressException()
    {
        Assert.Throws<MissingEmailAddressException>(() => new Email((MailAddress)null!));
    }

    [Fact]
    public void WithValidMailAddress_NormalizeAndSetValue()
    {
        var mailAddress = new MailAddress("  Test@Example.Com  ");

        var email = new Email(mailAddress);

        Assert.Equal("test@example.com", email.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void WithEmptyEmailString_ThrowEmptyEmailAddressException(string? input)
    {
        Assert.Throws<MissingEmailAddressException>(() => new Email(input!));
    }

    [Fact]
    public void WithInvalidEmailString_ThrowInvalidEmailAddressException()
    {
        Assert.Throws<InvalidEmailAddressException>(() => new Email("not-an-email"));
    }

    [Fact]
    public void WithValidEmailString_NormalizeAndSetValue()
    {
        var email = new Email("  Test@Example.Com  ");

        Assert.Equal("test@example.com", email.ToString());
    }
}