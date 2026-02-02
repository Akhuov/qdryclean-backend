using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Application.UseCases.Orders.Queries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("Order created successfully.", result);
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> DeleteOrderAsync(DeleteOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPut("{orderId:int}")]
        public async Task<IActionResult> UpdateOrderAsync(int orderId, UpdateOrderCommand command)
        {
            command.Id = orderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var command = new GetAllOrdersQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetByIdOrderAsync(int orderId)
        {
            var command = new GetByIdOrderQuery() { Id = orderId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        
    }

}
