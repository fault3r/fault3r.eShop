
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;

namespace CatalogManagementService.Application.EventHandlers
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
            _logger.LogInformation($"item with id {@event.Id} deleted.");

            //implement event handler
            return Task.CompletedTask;
        }
    }
}