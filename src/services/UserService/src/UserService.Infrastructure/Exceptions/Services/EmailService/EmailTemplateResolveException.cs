
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class EmailTemplateResolveException : InfrastructureException
{
    public EmailTemplateResolveException()
        : base("cannot resolve email template file") { }
}
