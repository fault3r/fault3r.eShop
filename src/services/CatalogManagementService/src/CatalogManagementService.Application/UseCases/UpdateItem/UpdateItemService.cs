
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.UpdateItem
{
    public class UpdateItemService : IUpdateItemService
    {
        private readonly IRepository _repository;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILoggerService<UpdateItemService> _logger;

        public UpdateItemService(IRepository repository, IEventPublisher eventPublisher,
            ILoggerService<UpdateItemService> logger)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, ItemDto? Item)> ExecuteAsync(string id, UpdateItemDto item)
        {
            await _logger.LogInformation("executing request..");
            if (!UpdateItemValidator.IsValid(id, item))
            {
                await _logger.LogInformation($"bad request!");
                return ((int)RepositoryResultCode.BadRequest, null);
            }
            var result = await _repository.UpdateAsync(new Item
            {
                Id = id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
            });
            if (result.Code == (int)RepositoryResultCode.Ok)
                await _eventPublisher.PublishAsync<ItemUpdatedEvent>(
                    ItemUpdatedEvent.Parse(result.Items.First()));
            await _logger.LogInformation("retrieved response.");
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.Parse(item)).FirstOrDefault());
        }
    }
}