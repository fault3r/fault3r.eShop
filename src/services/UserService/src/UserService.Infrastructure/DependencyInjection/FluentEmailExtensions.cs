
using System;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class FluentEmailExtensions
{
    public static IServiceCollection AddFluentEmailSmtp(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var setting = configuration
            .GetSection(nameof(FluentEmailSetting))
            .Get<FluentEmailSetting>()
                ?? throw new MissingFluentEmailSetting();

        services
            .AddFluentEmail(setting.Address,setting.Name)
            .AddRazorRenderer()
            .AddSmtpSender(new SmtpClient
            {
                Host = setting.Host,
                Port = setting.Port,
                EnableSsl = setting.EnableSsl,


            })

        return services;
    }
}