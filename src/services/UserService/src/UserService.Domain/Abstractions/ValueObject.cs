
using System;

namespace UserService.Domain.Abstractions;

public abstract record ValueObject<TValue>
{
    public abstract TValue Value { get; }
}