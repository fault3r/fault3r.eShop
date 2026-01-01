
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Services.EmailService;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class FluentEmailExtensions
{
    public static IServiceCollection AddFluentEmailService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var root = configuration[$"{nameof(AppSetting)}:ContentRoot"]
            ?? throw new MissingAppSettingException();

        var settings = configuration
            .GetSection(nameof(FluentEmailSetting))
            .Get<FluentEmailSetting>()
                ?? throw new MissingFluentEmailSettingException();

        services
            .AddFluentEmail(
                defaultFromEmail: settings.Email,
                defaultFromName: settings.Name
            )
            .AddSmtpSender(
                new SmtpClient
                {
                    Host = settings.Host,
                    Port = settings.Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        userName: settings.Email,
                        password: settings.Password
                    ),
                }
            );

        services.AddSingleton<IEmailTemplateResolver>(_ =>
        {
            var path = Path.Combine(root, "src", settings.TemplatesPath);
            return new EmailTemplateResolver(path);
        });

        services.AddSingleton<IEmailTemplateRenderer, EmailTemplateRenderer>();

        services.AddScoped<IEmailSender, FluentEmailSender>();

        return services;
    }
}