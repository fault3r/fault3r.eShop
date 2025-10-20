
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.UseCases.GetItems;
using CatalogManagementService.Application.UseCases.GetItem;
using CatalogManagementService.Application.UseCases.CreateItem;
using CatalogManagementService.Application.UseCases.UpdateItem;
using CatalogManagementService.Application.UseCases.DeleteItem;
using CatalogManagementService.Application.Interfaces;

namespace CatalogManagementService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        private readonly ILoggerService<CatalogController> _logger;

        public CatalogController(IMediator mediator,
            ILoggerService<CatalogController> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            await _logger.LogInformation("forwarding GetItemsQuery to mediator..");
            var (Code, Items) = await _mediator.Send(new GetItemsQuery());
            await _logger.LogInformation("retrieved GetItemsQuery response from mediator.");
            return Code switch
            {
                StatusCodes.Status200OK => Ok(Items),
                _ => StatusCode(Code),
            };
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] string id)
        {
            await _logger.LogInformation("forwarding GetItemQuery to mediator..");
            var (Code, Item) = await _mediator.Send(new GetItemQuery { Id = id });
            await _logger.LogInformation("retrieved GetItemQuery response from mediator.");
            return Code switch
            {
                StatusCodes.Status200OK => Ok(Item),
                StatusCodes.Status400BadRequest => BadRequest(),
                StatusCodes.Status404NotFound => NotFound(),
                _ => StatusCode(Code),
            };
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateItemDto item)
        {
            await _logger.LogInformation("forwarding CreateItemCommand to mediator..");
            var (Code, Item) = await _mediator.Send(new CreateItemCommand { Item = item });
            await _logger.LogInformation("retrieved CreateItemCommand response from mediator.");
            return Code switch
            {
                StatusCodes.Status201Created =>
                    CreatedAtAction(nameof(GetByIdAsync), new { id = Item.Id }, Item),
                _ => StatusCode(Code),
            };
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateItemDto item)
        {
            await _logger.LogInformation("forwarding UpdateItemCommand to mediator..");
            var (Code, Item) = await _mediator.Send(new UpdateItemCommand { Id = id, Item = item });
            await _logger.LogInformation("retrieved UpdateItemCommand response from mediator.");
            return Code switch
            {
                StatusCodes.Status200OK => Ok(Item),
                StatusCodes.Status400BadRequest => BadRequest(),
                StatusCodes.Status404NotFound => NotFound(),
                _ => StatusCode(Code),
            };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string id)
        {
            await _logger.LogInformation("forwarding DeleteItemCommand to mediator..");
            int Code = await _mediator.Send(new DeleteItemCommand { Id = id });
            await _logger.LogInformation("retrieved DeleteItemCommand response from mediator.");
            return Code switch
            {
                StatusCodes.Status204NoContent => NoContent(),
                StatusCodes.Status400BadRequest => BadRequest(),
                StatusCodes.Status404NotFound => NotFound(),
                _ => StatusCode(Code),
            };
        }
    }
}
