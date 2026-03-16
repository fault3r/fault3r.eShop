
using System;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UserService.Application.Messaging.Notification;
using UserService.Domain.Exceptions.Email;
using UserService.Domain.Factories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.CrossCutting;
using UserService.Infrastructure.CrossCutting.JsonSerializer;
using UserService.Infrastructure.Messaging.Notification;
using UserService.Infrastructure.Messaging.Outbox;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public async void TestName()
    {

        var obj1 = FullName.From(" Hamed ", "    Damaavandi");
        var obj2 = FullName.From("Hamed", "Damaavandi");
        Assert.True(obj1 == obj2);


    }
}