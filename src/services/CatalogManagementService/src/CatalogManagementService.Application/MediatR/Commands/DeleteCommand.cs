using System;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Commands
{
    public class DeleteCommand : IRequest<int>
    {
        public required string Id { get; set; }
    }
}