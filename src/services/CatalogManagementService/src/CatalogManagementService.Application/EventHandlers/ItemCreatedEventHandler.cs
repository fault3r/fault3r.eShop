
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;

namespace CatalogManagementService.Application.EventHandlers
{
    public class ItemCreatedEventHandler : IEventHandler<ItemCreatedEvent>
    {
        public Task Handle(ItemCreatedEvent @event)
        {
            //log
            Console.WriteLine($"***{nameof(ItemCreatedEventHandler)} is handling an event.");
            Console.WriteLine($"Item Created.\nId: {@event.Id}");
            return Task.CompletedTask;
        }

    }
}