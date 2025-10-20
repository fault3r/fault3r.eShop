
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.DeleteItem
{
    public class DeleteItemService : IDeleteItemService
    {
        private readonly IRepository _repository;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILoggerService<DeleteItemService> _logger;

        public DeleteItemService(IRepository repository, IEventPublisher eventPublisher,
            ILoggerService<DeleteItemService> logger)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<int> ExecuteAsync(string id)
        {
            await _logger.LogInformation("executing request..");
            if (!DeleteItemValidator.IsValid(id))
                return (int)RepositoryResultCode.BadRequest;
            var result = await _repository.DeleteAsync(id);
            if (result.Code == (int)RepositoryResultCode.NoContent)
                await _eventPublisher.PublishAsync<ItemDeletedEvent>(
                    new ItemDeletedEvent { Id = id });
            await _logger.LogInformation("retrieved response.");
            return result.Code;
        }
    }
}