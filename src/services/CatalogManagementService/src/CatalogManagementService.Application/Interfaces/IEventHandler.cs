
using System;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}