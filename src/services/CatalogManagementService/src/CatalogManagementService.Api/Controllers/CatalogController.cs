
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
            return StatusCode(Code, Items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] string id)
        {
            var (Code, Item) = await _mediator.Send(new GetByIdQuery { Id = id });
            if (Code == 404)
                return NotFound();
            return StatusCode(Code, Item);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateItemDto item)
        {
            var (Code, Item) = await _mediator.Send(new CreateCommand { Item = item });
            return StatusCode(Code, Item);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateItemDto item)
        {
            var (Code, Item) = await _mediator.Send(new UpdateCommand { Id = id, Item = item });
            return StatusCode(Code, Item);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string id)
        {
            int Code = await _mediator.Send(new DeleteCommand { Id = id });
            return StatusCode(Code);
        }
        
    }
}
