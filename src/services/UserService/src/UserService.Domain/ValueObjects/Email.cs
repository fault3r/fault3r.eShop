
using System;
using System.Net.Mail;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;

namespace UserService.Domain.ValueObjects;

public sealed class Email : ValueObject<Email>
{
    public MailAddress Value { get; }

    public Email(MailAddress mailAddress)
    {
        if (mailAddress is null)
            throw new EmptyEmailAddressException();

        Value = mailAddress;
    }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new EmptyEmailAddressException();

        var normalized = address
            .Trim()
            .ToLowerInvariant();
        if (!IsValid(normalized))
            throw new InvalidEmailAddressException(normalized);

        Value = new MailAddress(normalized);
    }

    private static bool IsValid(string address)
       => MailAddress.TryCreate(address, out MailAddress? _); 

    public static Email From(MailAddress mailAddress)
        => new(mailAddress);
        
    public static Email Parse(string address) 
        => new(address);

    public override string ToString()
        => Value.Address;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
