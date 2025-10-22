
using System;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.DTOs
{
    public record ItemDto(
        string Id,
        string Name,
        string Description,
        decimal Price,
        DateTime Updated);
}