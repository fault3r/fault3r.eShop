using System;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Commands
{
    public class DeleteItemCommand : IRequest<int>
    {
        public required string Id { get; set; }
    }
}