
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class CannotRenderEmailTemplateException : InfrastructureException
{
    public CannotRenderEmailTemplateException()
        : base("cannot render email template") { }
}
