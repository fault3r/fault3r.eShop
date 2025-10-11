
using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.UseCases.GetItems
{
    public class GetItemsQuery : IRequest<(int Code, IEnumerable<ItemDto> Items)> { }
    
}