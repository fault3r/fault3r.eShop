
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Mediator.Commands;
using CatalogManagementService.Application.Mediator.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogManagementService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/catalog")]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var (Code, Items) = await _mediator.Send(new GetItemsQuery());
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
            var (Code, Item) = await _mediator.Send(new GetItemQuery { Id = id });
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
            var (Code, Item) = await _mediator.Send(new CreateItemCommand { Item = item });
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
            var (Code, Item) = await _mediator.Send(new UpdateItemCommand { Id = id, Item = item });
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
            int Code = await _mediator.Send(new DeleteItemCommand { Id = id });
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
