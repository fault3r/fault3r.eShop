using System;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventPublisher
    {
        (bool Success, string Message) PublishAsync<TEvent>(TEvent @event) where TEvent : class;
    }
}