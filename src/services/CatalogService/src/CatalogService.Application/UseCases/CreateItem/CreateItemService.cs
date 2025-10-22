
using System;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DTOs;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Events;
using CatalogService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.UseCases.CreateItem
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
                Item: result.Items.Select(item => ItemDTOs.ToItemDto(item)).FirstOrDefault());
        }
    }
}