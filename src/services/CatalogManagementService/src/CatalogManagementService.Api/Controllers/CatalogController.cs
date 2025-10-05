
using System;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.MediatR.Commands;
using CatalogManagementService.Application.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogManagementService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/catalog")]
    [ApiVersion("1.0")]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;        

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var (Code, Items) = await _mediator.Send(new GetAllQuery());
            return Code switch
            {
                200 => Ok(Items),
                _ => StatusCode(Code),
            };
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] string id)
        {
            var (Code, Item) = await _mediator.Send(new GetByIdQuery { Id = id });
            return Code switch
            {
                200 => Ok(Item),
                400 => BadRequest(),
                404 => NotFound(),
                _ => StatusCode(Code),
            };
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateItemDto item)
        {
            var (Code, Item) = await _mediator.Send(new CreateCommand { Item = item });
            return Code switch
            {
                201 => CreatedAtAction(nameof(GetByIdAsync), new { id = Item.Id }, Item),
                _ => StatusCode(Code),
            };
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateItemDto item)
        {
            var (Code, Item) = await _mediator.Send(new UpdateCommand { Id = id, Item = item });
            return Code switch
            {
                200 => Ok(Item),
                400 => BadRequest(),
                404 => NotFound(),
                _ => StatusCode(Code),
            };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string id)
        {
            int Code = await _mediator.Send(new DeleteCommand { Id = id });
            return Code switch
            {
                204 => NoContent(),
                400 => BadRequest(),
                404 => NotFound(),
                _ => StatusCode(Code),
            };
        }
        
    }
}
