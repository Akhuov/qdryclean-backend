using MediatR;
using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.UseCases.Dashboard.Queries;

namespace QDryClean.Api.Controllers
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("orders/summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string from, [FromQuery] string to)
            => Ok(await _mediator.Send(new OrdersSummaryQuery() { From = from, To = to }));
    }
}
