
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using MediatR;

namespace CatalogManagementService.Application.UseCases.CreateItem
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly ICreateItemService _service;

        private readonly  ILoggerService<CreateItemCommandHandler> _logger;

        public CreateItemCommandHandler(ICreateItemService service,
            ILoggerService<CreateItemCommandHandler> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, ItemDto? Item)> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            await _logger.LogInformation("forwarding request to service..");
            var (Code, Item) = await _service.ExecuteAsync(request.Item);
            await _logger.LogInformation("retrieved response from service.");
            return (Code, Item);
        }
    }
}