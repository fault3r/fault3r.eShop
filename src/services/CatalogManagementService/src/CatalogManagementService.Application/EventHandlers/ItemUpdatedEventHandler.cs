using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;

namespace CatalogManagementService.Application.EventHandlers
{
    public class ItemUpdatedEventHandler : IEventHandler<ItemUpdatedEvent>
    {
        public Task HandleAsync(ItemUpdatedEvent @event)
        {
            //log
            Console.WriteLine($"***{nameof(ItemUpdatedEventHandler)} is handling an event.");            
            Console.WriteLine($"Item Updated.\nId: {@event.Id}");
            return Task.CompletedTask;
        }
    }
}