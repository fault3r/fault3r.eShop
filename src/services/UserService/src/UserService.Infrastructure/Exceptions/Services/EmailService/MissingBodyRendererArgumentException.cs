
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingBodyRendererArgumentException : InfrastructureException
{
    public MissingBodyRendererArgumentException()
        : base("razor email body renderer arguments are required") { }
}
