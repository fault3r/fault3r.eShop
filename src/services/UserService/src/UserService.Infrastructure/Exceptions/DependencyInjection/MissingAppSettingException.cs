
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingAppSettingException : InfrastructureException
{
    public MissingAppSettingException()
        : base("missing app setting") { }
}
