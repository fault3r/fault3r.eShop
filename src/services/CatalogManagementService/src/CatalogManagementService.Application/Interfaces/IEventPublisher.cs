using System;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event);
    }
}