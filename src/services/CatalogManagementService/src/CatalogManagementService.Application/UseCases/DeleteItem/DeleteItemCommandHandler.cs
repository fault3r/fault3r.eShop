using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.Mediator.Commands;
using MediatR;

namespace CatalogManagementService.Application.Mediator.Handlers.Commands
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, int>
    {
        private readonly IItemsService _ItemsService;

        public DeleteItemCommandHandler(IItemsService ItemsService)
        {
            _ItemsService = ItemsService;
        }

        public async Task<int> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            return await _ItemsService.DeleteAsync(request.Id);
        }
    }
}