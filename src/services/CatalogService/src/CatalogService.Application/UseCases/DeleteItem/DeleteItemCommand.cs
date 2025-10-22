
using System;
using MediatR;

namespace CatalogService.Application.UseCases.DeleteItem
{
    public class DeleteItemCommand : IRequest<int>
    {
        public required string Id { get; set; }
        
    }
}