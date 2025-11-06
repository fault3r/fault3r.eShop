
using System;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.Common;

public sealed record Identity(Guid Value)
{
    public static Identity New()
        => new(Guid.NewGuid());

    public static Identity From(string guid)
    {
        try
        {
            return new(Guid.Parse(guid));
        }
        catch
        {
            throw new DomainException($"invalid Guid: {guid}");
        }
    }

    public override string ToString()
        => Value.ToString();
}
