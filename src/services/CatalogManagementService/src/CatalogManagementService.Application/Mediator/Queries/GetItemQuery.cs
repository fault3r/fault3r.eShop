using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Queries
{
    public class GetItemQuery : IRequest<(int Code, ItemDto? Item)>
    {
        public required string Id { get; set; }        
    }
}