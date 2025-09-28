using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Commands
{
    public class CreateItemCommand : IRequest<ItemDto?>
    {
        public required CreateItemDto Item { get; set; }
    }
}