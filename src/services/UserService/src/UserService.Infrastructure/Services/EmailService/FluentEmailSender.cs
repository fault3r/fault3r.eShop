
using System;
using FluentEmail.Core;
using UserService.Application.Services.EmailService;
using UserService.Domain.Common;

namespace UserService.Infrastructure.Services.EmailService;

public class FluentEmailSender(IFluentEmail fluentEmail) : IEmailSender
{
    private readonly IFluentEmail _mailSender = fluentEmail;

    public async Task<Result> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        var response = await _mailSender
            .To(to)
            .Subject(subject)
            .Body(body, isHtml: true)
            .SendAsync(cancellationToken);

        return !response.Successful
            ? Result.Failure(string.Join(", ", response.ErrorMessages))
            : Result.Success();
    }
}
