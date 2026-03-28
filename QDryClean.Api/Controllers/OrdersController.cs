using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Application.UseCases.Orders.Queries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderCommand command)
            => Created("Order created successfully.", await _mediator.Send(command));


        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> DeleteOrderAsync(DeleteOrderCommand command)
            => Ok(await _mediator.Send(command));


        [HttpPut("{orderId:int}")]
        public async Task<IActionResult> UpdateOrderAsync(int orderId, [FromBody] UpdateOrderCommand command)
        {
            command.Id = orderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync([FromQuery] GetAllOrdersQuery query)
            => Ok(await _mediator.Send(query));


        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetByIdOrderAsync(int orderId)
            => Ok(await _mediator.Send(new GetByIdOrderQuery() { Id = orderId }));
    }
}
