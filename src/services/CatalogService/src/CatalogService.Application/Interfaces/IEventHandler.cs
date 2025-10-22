
using System;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}