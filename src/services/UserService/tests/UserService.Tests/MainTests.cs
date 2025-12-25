
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using UserService.Application.Services.EmailService;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Services.EmailService;
using UserService.Infrastructure.Services.EmailService.Templates.Models;
using UserService.Infrastructure.Settings;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var aa = Status.From("Locked");
        var sss = Status.From("33");

    }
}
