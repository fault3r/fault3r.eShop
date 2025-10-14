
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.GetItems
{
    public class GetItemsService(
        IRepository repository) : IGetItemsService
    {
        private readonly IRepository _repository = repository;

        public async Task<(int Code, IEnumerable<ItemDto> Items)> ExecuteAsync()
        {
            var result = await _repository.GetAllAsync();
            return (
                Code: result.Code,
                Items: result.Items.Select(item => ItemDTOs.Parse(item)));
        }
        
    }
}