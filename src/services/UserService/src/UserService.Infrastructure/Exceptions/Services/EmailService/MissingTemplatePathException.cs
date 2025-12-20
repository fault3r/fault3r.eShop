
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingTemplatePathException : InfrastructureException
{
    public MissingTemplatePathException()
        : base("template path is required") { }
}
