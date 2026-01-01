
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingRedisSettingException : InfrastructureException
{
    public MissingRedisSettingException()
        : base("missing remote directory server (redis) setting") { }
}
