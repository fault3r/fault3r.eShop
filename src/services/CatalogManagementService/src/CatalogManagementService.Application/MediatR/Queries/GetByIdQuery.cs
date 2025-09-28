using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Queries
{
    public class GetByIdQuery : IRequest<ItemDto?>
    {
        public required string Id { get; set; }        
    }
}