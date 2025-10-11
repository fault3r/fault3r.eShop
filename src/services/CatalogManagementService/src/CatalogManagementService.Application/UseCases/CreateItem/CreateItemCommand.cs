using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Commands
{
    public class CreateItemCommand : IRequest<(int Code, ItemDto? Item)>
    {
        public required CreateItemDto Item { get; set; }
    }
}