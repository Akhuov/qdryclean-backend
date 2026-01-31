using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Charges.Commands
{
    public class CreateChargeCommand : IRequest<ApiResponse<ChargeDto>>
    {
        public decimal Cost { get; set; }
        public string? Name { get; set; }
    }
}