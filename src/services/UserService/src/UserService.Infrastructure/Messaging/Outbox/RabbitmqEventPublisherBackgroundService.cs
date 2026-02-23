
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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var logger = Log.ForContext<RabbitmqEventPublisherBackgroundService>();

        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var _outbox = scope.ServiceProvider.GetRequiredService<IEventOutbox>();
            var _publisher = scope.ServiceProvider.GetRequiredService<RabbitmqEventPublisher>();

            try
            {
                var messages = await _outbox.DequeueAsync(count: 1, cancellationToken);

                if (!messages.Any())
                {
                    await Task.Delay(200, cancellationToken);
                    continue;
                }

                logger.Information("Retrieved {Count} message(s).", messages.Count());

                foreach (var message in messages)
                {
                    await _publisher.PublishAsync(message, cancellationToken);

                    // messages may be published more than once
                    // but consumer will ignore duplicates because it is idempotent

                    await _outbox.MarkAsProcessedAsync(message.Id, cancellationToken);

                    logger.Information("{Correlation} {Type} Published.", message.CorrelationId, message.Type);

                    await Task.Delay(100, cancellationToken);
                }
            }
            // OperationCanceledException handles internally by BackgroundService
            catch (Microsoft.EntityFrameworkCore.Storage.RetryLimitExceededException)
            {
                logger.Error("EFCore connection error, Reconnectiong…");
                await Task.Delay(1000, cancellationToken);

            }
            catch (RabbitMQ.Client.Exceptions.AlreadyClosedException)
            {
                logger.Error("RabbitMQ connection error!");
                await Task.Delay(1000, cancellationToken);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An exception occurred!");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
