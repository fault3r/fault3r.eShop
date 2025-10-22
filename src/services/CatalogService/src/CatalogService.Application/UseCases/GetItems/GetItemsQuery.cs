
using System;
using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.UseCases.GetItems
{
    public class GetItemsQuery : IRequest<(int Code, IEnumerable<ItemDto> Items)> { }
    
}