
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Events;

namespace CatalogService.Application.EventHandlers
{
    public class ItemDeletedEventHandler : IEventHandler<ItemDeletedEvent>
    {
        private readonly ILoggerService<ItemDeletedEventHandler> _logger;

        public ItemDeletedEventHandler(ILoggerService<ItemDeletedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(ItemDeletedEvent @event)
        {
            _logger.LogInformation($"delete item with id '{@event.Id}'");

            //implement event handler
            return Task.CompletedTask;
        }
    }
}