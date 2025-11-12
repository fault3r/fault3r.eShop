
using System;

namespace AccountService.Domain.Exceptions.Persistence;

public class MissingOutBoxException : DomainException
{
    public MissingOutBoxException() : base("Outbox is required"){}
}