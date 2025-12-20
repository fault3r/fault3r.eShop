
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Services.EmailService;
using UserService.Infrastructure.Settings;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public async void TestName()
    {
        
        // var resolver = new EmailTemplateResolver("/home/hamed-damavandi/Documents/fault3r.eShop/src/services/UserService/src/UserService.Infrastructure/");
        // var temp = await resolver.GetWelcome();
        
    }
}
