
using System;
using MediatR;
using UserService.Application.Messaging.Notifications;
using UserService.Application.Services.EmailService;
using UserService.Application.Services.EmailService.EmailTemplateModels;

namespace UserService.Application.Messaging.NotificationHandlers;

public sealed class UserCreatedNotificationHandler(
    IEmailTemplateResolver resolver,
    IEmailTemplateRenderer renderer,
    IEmailSender sender
) : INotificationHandler<UserCreatedNotification>
{
    private readonly IEmailTemplateResolver _resolver = resolver;
    private readonly IEmailTemplateRenderer _renderer = renderer;
    private readonly IEmailSender _sender = sender;

    public async Task Handle(
        UserCreatedNotification notification,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(notification);

        var template = await _resolver.ResolveAsync(EmailTemplateType.Welcome, ct);
        var model = new WelcomeModel(notification.FullName);

        var body = await _renderer.RenderAsync(template, model, ct);

        await _sender.SendAsync(
            to: notification.Email,
            subject: "Wewlcome",
            body: body,
            ct: ct
        );
    }
}
