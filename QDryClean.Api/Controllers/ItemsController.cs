using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Items.Commands;
using QDryClean.Application.UseCases.Items.Querries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
    [Route("api/v1/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync(CreateItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("ItemType created successfully.", result);
        }

        [HttpDelete("{itemId:int}")]
        public async Task<IActionResult> DeleteItemAsync(int itemId)
        {
            var result = await _mediator.Send(new SoftDeleteItemCommand() { Id = itemId });
            return Ok(result);
        }

        [HttpPatch("status/{itemId:int}")]
        public async Task<IActionResult> PatchItemAsync(int itemId, PatchItemStatusCommand command)
        {
            command.Id = itemId;
            return Ok(await _mediator.Send(command));
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
