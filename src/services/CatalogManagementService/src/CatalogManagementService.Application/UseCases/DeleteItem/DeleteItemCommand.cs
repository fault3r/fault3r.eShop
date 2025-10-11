
using System;
using MediatR;

namespace CatalogManagementService.Application.UseCases.DeleteItem
{
    public class DeleteItemCommand : IRequest<int>
    {
        public required string Id { get; set; }
        
    }
}