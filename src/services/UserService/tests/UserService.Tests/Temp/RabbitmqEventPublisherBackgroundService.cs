
// using System;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Serilog;
// using UserService.Domain.Messaging.Outbox;
// using UserService.Infrastructure.Messaging.EventBus;

// namespace UserService.Infrastructure.Messaging.Outbox;

// public sealed class RabbitmqEventPublisherBackgroundService(
//     IServiceScopeFactory serviceScopeFactory
// ) : BackgroundService
// {
//     private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

//     protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
//     {
//         var logger = Log.ForContext<RabbitmqEventPublisherBackgroundService>();

//         while (!cancellationToken.IsCancellationRequested)
//         {
//             await using var scope = _scopeFactory.CreateAsyncScope();
//             var _outbox = scope.ServiceProvider.GetRequiredService<IEventOutbox>();
//             var _publisher = scope.ServiceProvider.GetRequiredService<RabbitmqEventPublisher>();

//             try
//             {
//                 var messages = await _outbox.DequeueAsync(count: 1, cancellationToken);

//                 if (!messages.Any())
//                 {
//                     await Task.Delay(200, cancellationToken);
//                     continue;
//                 }

//                 logger.Information("Retrieved {Count} message(s).", messages.Count());

//                 foreach (var message in messages)
//                 {
//                     await _publisher.PublishAsync(message, cancellationToken);

//                     await _outbox.MarkAsProcessedAsync(message.Id, cancellationToken);

//                     logger.Information("{Correlation} {Type} Published.", message.CorrelationId, message.Type);

//                     await Task.Delay(100, cancellationToken);
//                 }
//             }
//             catch (Microsoft.EntityFrameworkCore.Storage.RetryLimitExceededException)
//             {
//                 logger.Error("EFCore connection error, Reconnectiong…");

//                 await Task.Delay(1000, cancellationToken);
//             }
//             catch (RabbitMQ.Client.Exceptions.AlreadyClosedException)
//             {
//                 logger.Error("RabbitMQ connection error!");
                
//                 await Task.Delay(1000, cancellationToken);
//             }
//             catch (Exception ex)
//             {
//                 logger.Error(ex, "An unexpected exception occurred!");
                
//                 await Task.Delay(1000, cancellationToken);
//             }
//         }
//     }
// }
