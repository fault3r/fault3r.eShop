using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Queries
{
    public class GetItemsQuery : IRequest<(int Code, IEnumerable<ItemDto> Items)> { }
}