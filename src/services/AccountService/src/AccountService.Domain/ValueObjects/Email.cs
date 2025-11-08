
using System;
using System.Net.Mail;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Email;

namespace AccountService.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Address { get; }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new MissingEmailException();
        
        var normalized = address.Trim().ToLowerInvariant();
        if (!IsValidAddress(normalized))
            throw new InvalidEmailException(normalized);

        Address = normalized;
    }

    public static bool IsValidAddress(string address)
       => MailAddress.TryCreate(address, out MailAddress? @out);

    public override string ToString()
        => Address;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}
