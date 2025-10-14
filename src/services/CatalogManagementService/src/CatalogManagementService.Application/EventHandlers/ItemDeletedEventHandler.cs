
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;

namespace CatalogManagementService.Application.EventHandlers
{
    public class ItemDeletedEventHandler : IEventHandler<ItemDeletedEvent>
    {
        public Task Handle(ItemDeletedEvent @event)
        {
            Console.WriteLine($"***Item Deleted.\nId: {@event.Id}");
            return Task.CompletedTask;
        }
    }
}