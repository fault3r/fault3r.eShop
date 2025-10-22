
using System;
using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.UseCases.CreateItem
{
    public class CreateItemCommand : IRequest<(int Code, ItemDto? Item)>
    {
        public required CreateItemDto Item { get; set; }
    }
}