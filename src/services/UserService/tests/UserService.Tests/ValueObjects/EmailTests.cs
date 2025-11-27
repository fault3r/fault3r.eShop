
using System;
using System.Net.Mail;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class EmailTests
{

    [Fact]
    public void WithNullMailAddress_ThrowException()
    {
        Assert.Throws<EmptyEmailAddressException>(() => new Email((MailAddress)null!));
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
    [InlineData("not-an-email")]
    public void WithEmptyOrInvalidEmailString_ThrowException(string input)
    {
        Assert.Throws<EmptyEmailAddressException>(() => new Email(input));
    }

    [Fact]
    public void WithValidEmailString_NormalizeAndSetValue()
    {
        var email = new Email("  Test@Example.Com  ");

        Assert.Equal("test@example.com", email.ToString());
    }
}