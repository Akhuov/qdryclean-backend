using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.ItemCategories.Commands;
using QDryClean.Application.UseCases.ItemCategories.Querries;
using QDryClean.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/item-categories")]
    [ApiController]
    public class ItemCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateItemCategoryAsync(CreateItemCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("User created successfully.", result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{itemCategoryId:int}")]
        public async Task<IActionResult> DeleteItemCategoryAsync(int itemCategoryId)
        {
            var result = await _mediator.Send(new SoftDeleteItemCategoryCommand() { Id = itemCategoryId });
            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPut("{itemCategoryId:int}")]
        public async Task<IActionResult> UpdateItemCategoryAsync(int itemCategoryId, UpdateItemCategoryCommand command)
        {
            command.Id = itemCategoryId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItemCategoriesAsync()
        {
            var command = new GetAllItemCategoriesQuerry();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{itemCategoryId:int}")]
        public async Task<IActionResult> GetByIdItemCategoryAsync(int itemCategoryId)
        {
            var command = new GetByIdItemCategoryQuerry() { Id = itemCategoryId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
