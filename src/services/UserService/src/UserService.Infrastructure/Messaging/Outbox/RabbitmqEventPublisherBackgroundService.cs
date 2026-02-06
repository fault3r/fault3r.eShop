
using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UserService.Domain.Messaging.Outbox;
using UserService.Infrastructure.Messaging.EventBus;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class RabbitmqEventPublisherBackgroundService(
    IServiceScopeFactory serviceScopeFactory
) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

    private static async Task SomeSecondsAsync(int second = 5)
        => await Task.Delay(TimeSpan.FromSeconds(second));

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var outbox = scope.ServiceProvider.GetRequiredService<IEventOutbox>();
        var publisher = scope.ServiceProvider.GetRequiredService<RabbitmqEventPublisher>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messages = await outbox.DequeueAsync(cancellationToken);

                Log.Information("{Name} Successfully retrieved {Count} message(s).",
                    nameof(RabbitmqEventPublisherBackgroundService), messages.Count());

                if (!messages.Any()) continue;

                foreach (var message in messages)
                {
                    await publisher.PublishAsync(message, cancellationToken);

                    await outbox.MarkAsProcessedAsync(message.Id, cancellationToken);

                    Log.Information("{Name} {Correlation} Successfully sent message {MessageId}.",
                        nameof(RabbitmqEventPublisherBackgroundService), message.CorrelationId, message.Id);

                    await SomeSecondsAsync(1);
                }
            }
            catch (RetryLimitExceededException)
            {
                Log.Error(nameof(RabbitmqEventPublisherBackgroundService)
                    + " Cannot connect to the messages database, Reconnectiong…");
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(RabbitmqEventPublisherBackgroundService)
                    + " An unhandled exception occurred!");
            }
            finally { await SomeSecondsAsync(); }
        }
    }
}
