using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Commands
{
    public class UpdateItemCommand : IRequest<ItemDto?>
    {
        public required string Id  { get; set; }

        public required UpdateItemDto Item { get; set; }
    }
}