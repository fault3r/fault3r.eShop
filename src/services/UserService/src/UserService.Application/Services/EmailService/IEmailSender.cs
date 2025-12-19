
using System;
using UserService.Domain.Common;

namespace UserService.Application.Services.EmailService;

public interface IEmailSender
{
    Task<Result> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default
    );
}
