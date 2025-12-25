
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class EmailBodyRenderException : InfrastructureException
{
    public EmailBodyRenderException()
        : base("cannot render email template body") { }
}
