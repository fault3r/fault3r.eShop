
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

        var logger = Log.ForContext<RabbitmqEventPublisherBackgroundService>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messages = await _outbox.DequeueAsync(count: 5, cancellationToken);

                if (!messages.Any()) continue;

                logger.Information("Retrieved {Count} message(s).", messages.Count());

                foreach (var message in messages)
                {
                    await _publisher.PublishAsync(message, cancellationToken);

                    await _outbox.MarkAsProcessedAsync(message.Id, cancellationToken);

                    logger.Information("{Correlation} {Type} Published.", message.CorrelationId, message.Type);

                    await SomeSecondsAsync(second: 1, cancellationToken);
                }
                await SomeSecondsAsync(second: 5, cancellationToken);
            }

            catch (Microsoft.EntityFrameworkCore.Storage.RetryLimitExceededException)
            {
                logger.Error("EFCore connection error, Reconnectiong…");
            }
            catch (RabbitMQ.Client.Exceptions.AlreadyClosedException)
            {
                logger.Error("RabbitMQ connection error!");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An exception occurred!");
            }
        }
    }

    private static async Task SomeSecondsAsync(
        int second = 5,
        CancellationToken cancellationToken = default)
    => await Task.Delay(TimeSpan.FromSeconds(second), cancellationToken);
}
