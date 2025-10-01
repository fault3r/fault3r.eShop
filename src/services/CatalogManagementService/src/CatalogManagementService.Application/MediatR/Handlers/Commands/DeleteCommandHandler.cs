using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Commands;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Commands
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, int>
    {
        private readonly ICatalogService _catalogService;

        public DeleteCommandHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<int> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            return await _catalogService.DeleteAsync(request.Id);
        }
    }
}