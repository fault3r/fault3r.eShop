
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.EventBus;

namespace CatalogManagementService.Api.HostedServices
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
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running RabbitMQ hosted service..");
            _rabbitmqEventSubscriber.StartSubscribeAsync();
            _logger.LogInformation("RabbitMQ hosted service runned successfully.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}