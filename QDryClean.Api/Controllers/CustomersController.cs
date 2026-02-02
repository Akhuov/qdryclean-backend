using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Customers.Commands;
using QDryClean.Application.UseCases.Customers.Queries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("User created successfully.", result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{customerId:int}")]
        public async Task<IActionResult> DeleteCustomerAsync(int customerId)
        {
            var result = await _mediator.Send(new SoftDeleteCustomerCommand() { Id = customerId });
            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPut("{customerId:int}")]
        public async Task<IActionResult> UpdateCustomerAsync(int customerId, UpdateCustomerCommand command)
        {
            command.Id = customerId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var result = await _mediator.Send(new GetAllCustomersQuery());
            return Ok(result);
        }

        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetByIdCustomerAsync(int customerId)
        {
            var result = await _mediator.Send(new GetByIdCustomerQuery() { Id = customerId });
            return Ok(result);
        }
    }
}
