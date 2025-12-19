
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingFluentEmailSettingException : InfrastructureException
{
    public MissingFluentEmailSettingException()
        : base("missing fluent email smtp setting") { }
}
