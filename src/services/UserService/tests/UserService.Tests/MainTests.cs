
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using UserService.Application.Services.EmailService;
using UserService.Application.Services.EmailService.EmailTemplateModels;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Exceptions;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Services.EmailService;
using UserService.Infrastructure.Settings;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        string template = @"<p>Hello {{Name}}</p><p>Welcome to our {{Service}}!</p>";

        var model = new {Name="hamed", Service="new service"};
        var renderer = new FluentEmailRazorBodyRenderer();
        var res = renderer.RenderAsync(template , model);
       
    }
}
