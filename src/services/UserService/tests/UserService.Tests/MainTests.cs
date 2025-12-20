
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Services.EmailService;
using UserService.Infrastructure.Services.EmailService.Templates.Models;
using UserService.Infrastructure.Settings;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public async void TestName()
    {
        string root = "/home/hamed-damavandi/Documents/fault3r.eShop/src/services/UserService/src";
        string temp = "UserService.Infrastructure/Services/EmailService/Templates";

        var resolver = new EmailTemplateResolver(root,temp);
        var template = await resolver.GetWelcome();
        var renderer = new FluentEmailRazorBodyRenderer();
        var rendered = await renderer
            .RenderAsync(
                template: template,
                model: new WelcomeModel("fault3r") as object
            );
    }
}
