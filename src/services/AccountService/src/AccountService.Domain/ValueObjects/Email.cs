
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
        if (!IsValid(normalized))
            throw new InvalidEmailException(normalized);

        Address = normalized;
    }

    private static bool IsValid(string input)
       => MailAddress.TryCreate(input, out MailAddress? @out);

    public static Email From(string input)
        => new(input);  

    public override string ToString()
        => Address;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}
