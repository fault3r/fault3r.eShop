using System;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Events;

namespace CatalogService.Application.EventHandlers
{
    public class ItemUpdatedEventHandler : IEventHandler<ItemUpdatedEvent>
    {
        private readonly ILoggerService<ItemUpdatedEventHandler> _logger;

        public ItemUpdatedEventHandler(ILoggerService<ItemUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemUpdatedEvent @event)
        {
            _logger.LogInformation($"update item with id '{@event.Id}'");

            //implement event handler
            return Task.CompletedTask;
        }
    }
}