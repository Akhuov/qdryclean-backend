using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Items.Commands;
using QDryClean.Application.UseCases.Items.Querries;
using QDryClean.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateItemAsync(CreateItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("ItemType created successfully.", result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{itemId:int}")]
        public async Task<IActionResult> DeleteItemAsync(int itemId)
        {
            var result = await _mediator.Send(new SoftDeleteItemCommand() { Id = itemId } );
            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPut]
        public async Task<IActionResult> UpdateItemAsync(UpdateItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItemTypesAsync()
        {
            var command = new GetAllItemsQuerry();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{itemId:int}")]
        public async Task<IActionResult> GetByIdItemAsync(int itemId)
        {
            var command = new GetByIdItemQuerry() { Id = itemId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
