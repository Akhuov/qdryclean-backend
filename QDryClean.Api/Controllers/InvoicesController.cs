using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Invoices.Commands;
using QDryClean.Application.UseCases.Invoices.Quarries;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/invoices")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> CreateInvoiceAsync(CreateInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Created("Invoice created successfully.", result);
        }
        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpDelete("{invoiceId:int}")]
        public async Task<IActionResult> DeleteInvoiceAsync(DeleteInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize(Roles = $"{nameof(UserRole.Receptionist)},{nameof(UserRole.Admin)}")]
        [HttpPut("{invoiceId:int}")]
        public async Task<IActionResult> UpdateInvoiceAsync(int invoiceId, UpdateInvoiceCommand command)
        {
            command.Id = invoiceId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            var command = new GetAllInvoicesCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{invoiceId:int}")]
        public async Task<IActionResult> GetByIdInvoiceAsync(int invoiceId)
        {
            var command = new GetByIdInvoiceCommand() { Id = invoiceId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
