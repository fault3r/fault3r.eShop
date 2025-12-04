
using System;
using UserService.Domain.Abstractions;

namespace UserService.Domain.ValueObjects;

public sealed record PasswordHash : ValueObject<string>
{
    public override string Value {get; }
}
