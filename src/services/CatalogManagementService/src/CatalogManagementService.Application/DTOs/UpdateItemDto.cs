using System;

namespace CatalogManagementService.Application.DTOs
{
    public record UpdateItemDto(
        string Name,
        string Description,
        decimal Price,
        IEnumerable<string> Pictures
    );
}