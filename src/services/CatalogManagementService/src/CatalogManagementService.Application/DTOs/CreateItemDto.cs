using System;

namespace CatalogManagementService.Application.DTOs
{
    public record CreateItemDto(
        string Name,
        string Description,
        decimal Price);
}