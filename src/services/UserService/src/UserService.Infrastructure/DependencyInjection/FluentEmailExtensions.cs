
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
        var rootPath = configuration[$"{nameof(AppSetting)}:RootPath"]
            ?? throw new MissingAppSettingException();

        var setting = configuration
            .GetSection(nameof(FluentEmailSetting))
            .Get<FluentEmailSetting>()
                ?? throw new MissingFluentEmailSettingException();

        services
            .AddFluentEmail(
                defaultFromEmail: setting.Email,
                defaultFromName: setting.Name
            )
            .AddSmtpSender(
                new SmtpClient
                {
                    Host = setting.Host,
                    Port = setting.Port,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        userName: setting.Email,
                        password: setting.Password
                    ),
                }
            )
            .AddRazorRenderer();

        services.AddScoped<IEmailSender, FluentEmailSender>();

        services.AddScoped<IEmailTemplateResolver>(provider =>
        {
            var templatesPath = Path.Combine(rootPath, setting.TemplatesPath);
            return new RazorEmailTemplateResolver(templatesPath);
        });

        services.AddScoped<IEmailBodyRenderer, FluentEmailRazorBodyRenderer>();

        return services;
    }
}