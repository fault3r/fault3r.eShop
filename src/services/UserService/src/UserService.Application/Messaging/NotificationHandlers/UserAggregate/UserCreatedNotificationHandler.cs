
using System;
using MediatR;
using UserService.Application.Messaging.Notifications.UserAggregate;
using UserService.Application.Services.EmailService;
using UserService.Application.Services.EmailService.EmailTemplateModels;

namespace UserService.Application.Messaging.NotificationHandlers.UserAggregate;

public sealed class UserCreatedNotificationHandler(
    IEmailTemplateResolver resolver,
    IEmailTemplateRenderer renderer,
    IEmailSender sender)
        : INotificationHandler<UserCreatedNotification>
{
    private readonly IEmailTemplateResolver _resolver = resolver;
    private readonly IEmailTemplateRenderer _renderer = renderer;
    private readonly IEmailSender _sender = sender;

    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        var template = await _resolver.ResolveAsync(EmailTemplateType.Welcome, cancellationToken);
        var model = new WelcomeModel(notification.FullName);
        var body = await _renderer.RenderAsync(template, model, cancellationToken);

        await _sender.SendAsync(notification.Email, "Wewlcome", body, cancellationToken);
    }
}
