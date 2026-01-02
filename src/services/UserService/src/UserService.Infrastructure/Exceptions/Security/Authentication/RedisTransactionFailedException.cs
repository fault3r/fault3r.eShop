
using System;

namespace UserService.Infrastructure.Exceptions.Security.Authentication;

public sealed class RedisTransactionFailedException : InfrastructureException
{
    public RedisTransactionFailedException()
        : base("cannot commit redis transaction") { }
}

