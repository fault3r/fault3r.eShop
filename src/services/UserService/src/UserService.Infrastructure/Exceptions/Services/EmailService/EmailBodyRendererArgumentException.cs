
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class EmailBodyRendererArgumentException : InfrastructureException
{
    public EmailBodyRendererArgumentException()
        : base("email body renderer arguments are required") { }
}
