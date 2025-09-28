using System;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Commands
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
}