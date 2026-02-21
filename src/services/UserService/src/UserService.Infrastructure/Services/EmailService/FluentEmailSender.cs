
using System;
using FluentEmail.Core;
using UserService.Application.Services.EmailService;
using UserService.Domain.Contracts;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class FluentEmailSender(
    IFluentEmail fluentEmail
) : IEmailSender
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;

    public async Task<Result> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(to);
        ArgumentException.ThrowIfNullOrEmpty(subject);
        ArgumentException.ThrowIfNullOrEmpty(body);

        var response = await _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body, isHtml: true)
            .SendAsync(cancellationToken);

        if (!response.Successful)
            throw new CannotSendEmailException(to);

        return Result.Success();
    }
}
