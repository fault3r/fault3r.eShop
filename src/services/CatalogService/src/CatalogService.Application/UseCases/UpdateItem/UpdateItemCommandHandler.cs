
using System;
using CatalogService.Application.DTOs;
using CatalogService.Application.Interfaces;
using MediatR;

namespace CatalogService.Application.UseCases.UpdateItem
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, (int Code, ItemDto? Item)>
    {
        private readonly IUpdateItemService _service;

        private readonly ILoggerService<UpdateItemCommandHandler> _logger;

        public UpdateItemCommandHandler(IUpdateItemService service,
            ILoggerService<UpdateItemCommandHandler> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, ItemDto? Item)> Handle(UpdateItemCommand request, CancellationToken ct)
        {
            await _logger.LogInformation("forwarding request to service..");
            var (Code, Item) = await _service.ExecuteAsync(request.Id, request.Item);
            await _logger.LogInformation("retrieved response from service.");
            return (Code, Item);
        }
    }
}