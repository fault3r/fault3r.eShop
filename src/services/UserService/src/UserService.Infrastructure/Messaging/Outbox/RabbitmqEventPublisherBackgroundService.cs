
using System;
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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var _outbox = scope.ServiceProvider.GetRequiredService<IEventOutbox>();
        var _publisher = scope.ServiceProvider.GetRequiredService<RabbitmqEventPublisher>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messages = await _outbox.DequeueAsync(count: 5, cancellationToken);

                if (!messages.Any()) continue;

                Log.Information("{Name} Successfully retrieved {Count} message(s).",
                    nameof(RabbitmqEventPublisherBackgroundService), messages.Count());

                foreach (var message in messages)
                {
                    await _publisher.PublishAsync(message, cancellationToken);

                    await _outbox.MarkAsProcessedAsync(message.Id, cancellationToken);

                    Log.Information("{Name} {Correlation} Successfully sent message '{MessageId}'.",
                        nameof(RabbitmqEventPublisherBackgroundService), message.CorrelationId, message.Id);

                    await SomeSecondsAsync(second: 5, cancellationToken);
                }
            }

            catch (Microsoft.EntityFrameworkCore.Storage.RetryLimitExceededException)
            {
                Log.Error(nameof(RabbitmqEventPublisherBackgroundService)
                    + " Cannot connect to the messages database, Reconnectiong…");
            }
            catch (RabbitMQ.Client.Exceptions.AlreadyClosedException)
            {
                Log.Error(nameof(RabbitmqEventPublisherBackgroundService)
                    + " Failed to connect to the messages publisher!");
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(RabbitmqEventPublisherBackgroundService)
                    + " An unhandled exception occurred!");
            }

            finally
            {
                await SomeSecondsAsync(second: 5, cancellationToken);
            }
        }
    }

    private static async Task SomeSecondsAsync(
        int second = 5,
        CancellationToken cancellationToken = default)
    => await Task.Delay(TimeSpan.FromSeconds(second), cancellationToken);
}
