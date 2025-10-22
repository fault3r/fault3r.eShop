
using System;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CatalogService.Application.UseCases.GetItems
{
    public class GetItemsService : IGetItemsService
    {
        private readonly IRepository _repository ;

        private readonly ILoggerService<GetItemsService> _logger ;

        public GetItemsService(IRepository repository,
            ILoggerService<GetItemsService> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, IEnumerable<ItemDto> Items)> ExecuteAsync()
        {
            await _logger.LogInformation("executing request..");
            var result = await _repository.GetAllAsync();
            await _logger.LogInformation("retrieved response.");
            return (
                Code: result.Code,
                Items: result.Items.Select(item => ItemDTOs.ToItemDto(item)));
        }
    }
}