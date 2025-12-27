
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingTemplateResolverArgumentException : InfrastructureException
{
    public MissingTemplateResolverArgumentException()
        : base("template resolver arguments are required") { }
}
 