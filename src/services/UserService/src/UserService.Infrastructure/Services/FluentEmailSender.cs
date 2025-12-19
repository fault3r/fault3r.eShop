
using System;
using UserService.Domain.Common;
using UserService.Domain.Services;

namespace UserService.Infrastructure.Services;

public class FluentEmailSender : IEmailSender
{
    public Task<Result> SendAsync()
    {
        throw new NotImplementedException();
    }
}
