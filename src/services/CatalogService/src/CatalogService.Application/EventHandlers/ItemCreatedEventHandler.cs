
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Events;

namespace CatalogService.Application.EventHandlers
{
    public class ItemCreatedEventHandler : IEventHandler<ItemCreatedEvent>
    {
        private readonly ILoggerService<ItemCreatedEventHandler> _logger;

        public ItemCreatedEventHandler(ILoggerService<ItemCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemCreatedEvent @event)
        {
            _logger.LogInformation($"create new item with id '{@event.Id}'");

            //implement event handler
            return Task.CompletedTask;
        }
    }
}