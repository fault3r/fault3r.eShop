
using System;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DTOs;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.UseCases.GetItem
{
    public class GetItemService : IGetItemService
    {
        private readonly IRepository _repository;

        private readonly ILoggerService<GetItemService> _logger;

        public GetItemService(IRepository repository,
            ILoggerService<GetItemService> logger)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, ItemDto? Item)> ExecuteAsync(string id)
        {
            await _logger.LogInformation("executing request..");
            if (!GetItemValidator.IsValid(id))
            {
                await _logger.LogInformation($"bad request!");
                return ((int)RepositoryResultCode.BadRequest, null);
            }
            var result = await _repository.GetByIdAsync(id);
            await _logger.LogInformation("retrieved response.");
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.Parse(item)).FirstOrDefault());
        }
    }
}