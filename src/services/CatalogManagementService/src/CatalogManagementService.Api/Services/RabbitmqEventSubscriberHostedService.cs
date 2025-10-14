
using System;
using CatalogManagementService.Infrastructure.EventBus;

namespace CatalogManagementService.Api.Services
{
    public class RabbitmqEventSubscriberHostedService(
        RabbitmqEventSubscriber rabbitmqEventSubscriber) : IHostedService
    {
        private readonly RabbitmqEventSubscriber _rabbitmqEventSubscriber = rabbitmqEventSubscriber;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //log
            Console.WriteLine($"***{nameof(RabbitmqEventSubscriberHostedService)} is starting in the background..");            
            await _rabbitmqEventSubscriber.Subscribe();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}