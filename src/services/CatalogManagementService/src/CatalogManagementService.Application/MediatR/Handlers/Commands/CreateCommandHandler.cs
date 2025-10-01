using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Application.MediatR.Commands;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Handlers.Commands
{
    public class CreateCommandHandler : IRequestHandler<CreateCommand, (int Code, ItemDto? Item)>
    {
        private readonly ICatalogService _catalogService;

        public CreateCommandHandler(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public async Task<(int Code, ItemDto? Item)> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            return await _catalogService.CreateAsync(request.Item);
        }
    }
}