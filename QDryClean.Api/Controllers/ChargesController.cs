using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Charges.Commands;
using QDryClean.Application.UseCases.Charges.Quarries;
using QDryClean.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/charges")]
    [ApiController]
    public class ChargesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChargesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateChargeAsync(CreateChargeCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("Charge created successfully.", result);
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{chargeId:int}")]
        public async Task<IActionResult> DeleteChargeAsync(int chargeId)
        {
            var result = await _mediator.Send(new SoftDeleteChargeCommand() { Id = chargeId});
            return Ok(result);
        }


        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPut]
        public async Task<IActionResult> UpdateChargeAsync(UpdateChargeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllChargesAsync()
        {
            var command = new GetAllChargesCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("{chargeId:int}")]
        public async Task<IActionResult> GetByIdChargeAsync(int chargeId)
        {
            var result = await _mediator.Send(new GetByIdChargeCommand() { Id = chargeId });
            return Ok(result);
        }
    }
}
