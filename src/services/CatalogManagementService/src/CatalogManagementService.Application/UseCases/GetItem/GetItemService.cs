
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.UseCases.GetItem
{
    public class GetItemService(
        IRepository repository)    
    {
        private readonly IRepository _repository = repository;

        public async Task<(int Code, ItemDto? Item)> ExecuteAsync(string id)
        {
            var result = await _repository.GetByIdAsync(id);
            return (
                Code: result.Code,
                Item: result.Items.Select(item => ItemDTOs.Parse(item)).FirstOrDefault());
        }
        
    }
}