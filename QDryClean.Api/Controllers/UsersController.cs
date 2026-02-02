using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Users.Commands;
using QDryClean.Application.UseCases.Users.Quarries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("User created successfully.", result);
        }


        [HttpDelete("{userId:int}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {

            var result = await _mediator.Send(new DeleteUserCommand { Id = userId });
            return Ok("User deleted successfully.");
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> UpdateUserAsync(int userId, UpdateUserCommand command)
        {
            command.Id = userId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _mediator.Send(new GetAllUsersCommand());
            return Ok(result);
        }


        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            var result = await _mediator.Send(new GetByIdUserCommand { Id = userId });
            return Ok(result);
        }
    }
}
