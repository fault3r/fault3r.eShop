
using System;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Infrastructure.EventBus;

namespace CatalogManagementService.Api.Services
{
    public class RabbitmqEventSubscriberHostedService(
        RabbitmqEventSubscriber rabbitmqEventSubscriber) : IHostedService
    {
        private readonly RabbitmqEventSubscriber _rabbitmqEventSubscriber = rabbitmqEventSubscriber;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitmqEventSubscriber.Subscribe<ItemCreatedEvent>();
            _rabbitmqEventSubscriber.Subscribe<ItemCreatedEvent>();
            _rabbitmqEventSubscriber.Subscribe<ItemCreatedEvent>();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}