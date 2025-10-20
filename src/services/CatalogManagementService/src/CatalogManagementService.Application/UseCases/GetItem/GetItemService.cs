
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.GetItem
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
                return ((int)RepositoryResultCode.BadRequest, null);
            var result = await _repository.GetByIdAsync(id);
            await _logger.LogInformation("retrieved response.");
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.Parse(item)).FirstOrDefault());
        }
    }
}