
using System;

namespace CatalogService.Application.DTOs
{
    public record CreateItemDto(
        string Name,
        string Description,
        decimal Price);
        
}