
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class BodyRendererArgumentException : InfrastructureException
{
    public BodyRendererArgumentException()
        : base("email body renderer arguments are required") { }
}
