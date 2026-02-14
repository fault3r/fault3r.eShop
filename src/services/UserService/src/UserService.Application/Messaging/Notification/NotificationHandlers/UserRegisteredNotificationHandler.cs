
using System;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Messaging.Notification.Notifications;
using UserService.Application.Services.EmailService;
using UserService.Application.Services.EmailService.EmailTemplateModels;

namespace UserService.Application.Messaging.Notification.NotificationHandlers;

public sealed class UserRegisteredNotificationHandler(
    IEmailTemplateResolver resolver,
    IEmailTemplateRenderer renderer,
    IEmailSender sender,
    ILogger<UserRegisteredNotificationHandler> logger
) : INotificationHandler<UserRegisteredNotification>
{
    private readonly IEmailTemplateResolver _resolver = resolver;
    private readonly IEmailTemplateRenderer _renderer = renderer;
    private readonly IEmailSender _sender = sender;
    private readonly ILogger<UserRegisteredNotificationHandler> _logger = logger;

    public async Task Handle(
        UserRegisteredNotification notification,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);

        _logger.LogInformation("{Correlation} Sending welcome email to '{Email}' user…",
            notification.CorrelationId, notification.Email);

        var model = new WelcomeModel(notification.FullName);
        var template = await _resolver.ResolveAsync(EmailTemplateType.Welcome, cancellationToken);
        var body = await _renderer.RenderAsync(template, model, cancellationToken);
        await Task.Delay(20000, cancellationToken);
        await _sender.SendAsync(
            to: notification.Email,
            subject: "Wewlcome",
            body: body,
            cancellationToken: cancellationToken
        ); 

        _logger.LogInformation("{Correlation} Welcome email successfully sent.", notification.CorrelationId);
    }
}
