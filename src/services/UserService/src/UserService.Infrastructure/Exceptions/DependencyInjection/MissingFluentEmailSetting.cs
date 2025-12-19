
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public class MissingFluentEmailSetting : InfrastructureException
{
    public MissingFluentEmailSetting()
        : base("missing fluent email smtp setting") { }
}
