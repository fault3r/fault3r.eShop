
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.EventBus;

namespace CatalogService.Api.HostedServices
{
    public class RabbitmqEventSubscriberHostedService : IHostedService
    {
        private readonly RabbitmqEventSubscriber _rabbitmqEventSubscriber;

        private readonly ILoggerService<RabbitmqEventSubscriberHostedService> _logger;

        public RabbitmqEventSubscriberHostedService(RabbitmqEventSubscriber rabbitmqEventSubscriber,
            ILoggerService<RabbitmqEventSubscriberHostedService> logger)
        {
            _rabbitmqEventSubscriber = rabbitmqEventSubscriber;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("starting subscriber..");
            _rabbitmqEventSubscriber.StartSubscriberAsync();
            _logger.LogInformation("started successfully.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}