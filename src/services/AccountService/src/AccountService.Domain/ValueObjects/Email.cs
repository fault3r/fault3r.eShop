
using System;
using System.Text.RegularExpressions;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Address { get; private set; }

    public Email(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new DomainException("Email address is required");
        if (!IsValidEmail(address))
            throw new DomainException($"invalid Email address: {address}");
        Address = address.Trim().ToLowerInvariant();
    }

    public static bool IsValidEmail(string email)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(
            input: email,
            pattern: pattern,
            options: RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    public override string ToString()
        => Address;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}
