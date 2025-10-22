
using System;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Interfaces
{
    public interface IGetItemService
    {
        Task<(int Code, ItemDto? Item)> ExecuteAsync(string id);
    }
}
