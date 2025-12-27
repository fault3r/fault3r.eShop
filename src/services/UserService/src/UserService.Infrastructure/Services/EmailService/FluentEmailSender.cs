
using System;
using FluentEmail.Core;
using UserService.Application.Services.EmailService;
using UserService.Domain.Common;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public class FluentEmailSender(IFluentEmail fluentEmail) : IEmailSender
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;

    public async Task<Result> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        if (
            string.IsNullOrWhiteSpace(to) ||
            string.IsNullOrWhiteSpace(subject) ||
            string.IsNullOrWhiteSpace(body)
        )
            throw new EmailSenderArgumentException();

        var response = await _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body, isHtml: true)
            .SendAsync(cancellationToken);

        return !response.Successful
            ? Result.Failure(string.Join(", ", response.ErrorMessages))
            : Result.Success();
    }
}
