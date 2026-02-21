
using System;
using UserService.Domain.Contracts;

namespace UserService.Application.Services.EmailService;

public interface IEmailSender
{
    Task<Result> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken
    );
}
