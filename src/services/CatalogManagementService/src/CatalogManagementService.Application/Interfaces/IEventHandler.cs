
using System;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventHandler<ItemEvent>
    {
        Task Handle(ItemEvent @event);

    }
}