
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CatalogManagementService.Application.UseCases.CreateItem
{
    public class CreateItemService : ICreateItemService
    {
        private readonly IRepository _repository;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILoggerService<CreateItemService> _logger;
        
        public CreateItemService(IRepository repository, IEventPublisher eventPublisher,
            ILoggerService<CreateItemService> logger)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, ItemDto? Item)> ExecuteAsync(CreateItemDto item)
        {
            await _logger.LogInformation("executing request..");
            if (!CreateItemValidator.IsValid(item))
            {
                await _logger.LogInformation($"bad request!");
                return ((int)RepositoryResultCode.BadRequest, null);
            }  
            var result = await _repository.CreateAsync(new Item
            {
                Id = nameof(Item),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
            });
            if (result.Code == (int)RepositoryResultCode.Created)
                await _eventPublisher.PublishAsync<ItemCreatedEvent>(
                    ItemCreatedEvent.Parse(result.Items.First()));
            await _logger.LogInformation("retrieved response.");
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.Parse(item)).FirstOrDefault());
        }
    }
}