
using System;

namespace AccountService.Domain.Exceptions.Persistence;

public class MissingUnitOfWorkException : DomainException
{
    public MissingUnitOfWorkException() : base("UnitOfWork is required"){}
}
