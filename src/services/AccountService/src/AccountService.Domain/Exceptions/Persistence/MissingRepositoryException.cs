
using System;

namespace AccountService.Domain.Exceptions.Persistence;

public class MissingRepositoryException : DomainException
{
    public MissingRepositoryException() : base("Repository is required"){}
}
