
using System;
using UserService.Domain.Common;

namespace UserService.Domain.Services;

public interface IEmailSender
{
    Task<Result> SendAsync();
}
