
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;

namespace CatalogManagementService.Application.EventHandlers
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
            _logger.LogInformation($"item with id {@event.Id} created.");

            //implement event handler
            return Task.CompletedTask;
        }
    }
}