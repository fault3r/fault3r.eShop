using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Commands
{
    public class CreateCommand : IRequest<(int Code, ItemDto? Item)>
    {
        public required CreateItemDto Item { get; set; }
    }
}