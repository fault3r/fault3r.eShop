
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract record ValueObject<TValue>
    : IValueObject,  IValueObject<TValue>
{
    public abstract TValue Value { get; init; }
}