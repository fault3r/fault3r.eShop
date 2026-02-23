
using System;
using Polly;
using Polly.Timeout;
using Serilog;
using StackExchange.Redis;
using UserService.Application.Messaging.Notification;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Messaging.Notification;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public async void Test()
    {
        var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var timeout = Policy
            .TimeoutAsync(
                TimeSpan.FromMilliseconds(6000),
                TimeoutStrategy.Pessimistic
            );

        var retry = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 1,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(2)
            );

        var fallback = Policy
            .Handle<Exception>()
            .FallbackAsync(async (ct) =>
            {
                Console.WriteLine("operation failed!");
            });

        var policy = fallback.WrapAsync(retry).WrapAsync(timeout);

        await policy.ExecuteAsync(async ct =>
        {
            var op = Task.Delay(4000, CancellationToken.None);

            await Task.WhenAny(op, Task.Delay(Timeout.Infinite, ct));

            ct.ThrowIfCancellationRequested();

            await op;

        }, ct);
    }

    private static async Task<T> ExecuteWithCancellationAsync<T>(
        Task<T> redisTask,
        CancellationToken cancellationToken)
    {
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            timeoutCts.Token
        );

        var cancelTask = Task.Delay(Timeout.Infinite, cts.Token);

        var completed = await Task.WhenAny(redisTask, cancelTask);

        if (completed == cancelTask)
        {
            if (timeoutCts.IsCancellationRequested)
                throw new TimeoutException("operation exceeded 10 seconds!");

            throw new OperationCanceledException(cancellationToken);
        }

        return await redisTask;
    }
}