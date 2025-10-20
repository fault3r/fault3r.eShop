using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;

namespace CatalogManagementService.Application.EventHandlers
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