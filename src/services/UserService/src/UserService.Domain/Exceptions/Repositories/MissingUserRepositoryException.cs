
using System;

namespace UserService.Domain.Exceptions.Repositories;

public class MissingUserRepositoryException : DomainException
{
    public MissingUserRepositoryException()
        : base("user repository is required") { }
}