

using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class InvalidApiVersionException : InfrastructureException
{
    public InvalidApiVersionException()
        : base("invalid api version") { }
}
