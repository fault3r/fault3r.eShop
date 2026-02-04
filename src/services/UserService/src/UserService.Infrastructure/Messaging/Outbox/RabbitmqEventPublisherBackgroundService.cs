
using System;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UserService.Domain.Messaging.Outbox;

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

        Log.Information("Event outbox publisher background service started.");        

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                IEnumerable<OutboxMessage> messages = [];

                messages = await outbox.DequeueAsync(cancellationToken);

                if (!messages.Any()) continue;

                foreach (var message in messages)
                {
                    //publish
                    Console.WriteLine(message.Type);

                    await outbox.MarkAsPublishedAsync(message.Id, cancellationToken);
                }
            }

            catch(SocketException)
            {
                Log.Error(nameof(RabbitmqEventPublisherBackgroundService)
                    + " Failed to connect to the Postgres database, Reconnectiong…");
            }
            catch (Exception ex)
            {
                Log.Error(ex, nameof(RabbitmqEventPublisherBackgroundService)
                    + " An unhandled exception occurred!");
            }

            finally { await SomeSecondsAsync(); }
        }

        Log.Information("Event outbox publisher background service stopped.");
    }
}
