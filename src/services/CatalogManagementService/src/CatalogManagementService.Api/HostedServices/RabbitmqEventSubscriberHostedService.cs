
using System;
using CatalogManagementService.Infrastructure.EventBus;

namespace CatalogManagementService.Api.HostedServices
{
    public class RabbitmqEventSubscriberHostedService(
        RabbitmqEventSubscriber rabbitmqEventSubscriber) : IHostedService
    {
        private readonly RabbitmqEventSubscriber _rabbitmqEventSubscriber = rabbitmqEventSubscriber;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //log
            Console.WriteLine($"***{nameof(RabbitmqEventSubscriberHostedService)} is starting in the background..");
            _rabbitmqEventSubscriber.StartSubscribeAsync();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}