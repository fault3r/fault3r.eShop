
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public sealed class CannotRenderEmailTemplateException : InfrastructureException
{
    public CannotRenderEmailTemplateException()
        : base("cannot render email template") { }
}
