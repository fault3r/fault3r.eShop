
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingJwtSettingException : InfrastructureException
{
    public MissingJwtSettingException()
        : base("missing json web token setting") { }
}
