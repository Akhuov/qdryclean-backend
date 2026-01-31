using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.ItemTypes.Commands;
using QDryClean.Application.UseCases.ItemTypes.Quarries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/item-types")]
    [ApiController]
    public class ItemTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateItemTypeAsync(CreateItemTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("ItemType created successfully.", result);
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{itemTypeId:int}")]
        public async Task<IActionResult> DeleteItemTypeAsync(int itemTypeId)
        {
            var result = await _mediator.Send(new SoftDeleteItemTypeCommand() { Id = itemTypeId });
            return Ok(result);
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateItemTypeAsync(UpdateItemTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllItemTypesAsync()
        {
            var result = await _mediator.Send(new GetAllItemTypesCommand());
            return Ok(result);
        }


        [HttpGet("{itemTypeId:int}")]
        public async Task<IActionResult> GetByIdItemTypeAsync(int itemTypeId)
        {
            var result = await _mediator.Send(new GetByIdItemTypeCommand() { Id = itemTypeId });
            return Ok(result);
        }
    }
}
