
using System;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UserService.Application.Messaging.Notification;
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
        var ct = new CancellationToken();
        var tt = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var token = CancellationTokenSource.CreateLinkedTokenSource(ct, tt.Token);

        try
        {
            await TestClass.Download(token.Token);
        }
        catch
        {
            
        }

    }
}

public class TestClass
{
    public static Task Download(CancellationToken ct)
    {
        Task.Delay(7000, ct);
        throw new Exception("could not download!");
    }
}