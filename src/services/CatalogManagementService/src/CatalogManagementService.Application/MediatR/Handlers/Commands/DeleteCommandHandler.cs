using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Commands;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Commands
{
    public class DeleteCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly ICatalogService _catalogService;

        public DeleteCommandHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var result = await _catalogService.DeleteAsync(request.Id);
            return result.Success;
        }
    }
}