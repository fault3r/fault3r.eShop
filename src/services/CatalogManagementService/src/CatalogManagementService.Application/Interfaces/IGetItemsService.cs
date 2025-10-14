
using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IGetItemsService
    {
        Task<(int Code, IEnumerable<ItemDto> Items)> ExecuteAsync();
    }
}
