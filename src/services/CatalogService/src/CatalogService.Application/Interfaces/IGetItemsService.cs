
using System;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Interfaces
{
    public interface IGetItemsService
    {
        Task<(int Code, IEnumerable<ItemDto> Items)> ExecuteAsync();
    }
}
