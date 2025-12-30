
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
       
    }
}
