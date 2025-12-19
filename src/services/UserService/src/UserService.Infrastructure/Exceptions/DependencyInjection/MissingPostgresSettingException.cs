
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingPostgresSettingException : InfrastructureException
{
    public MissingPostgresSettingException()
        : base("missing postgres setting") { }
}
