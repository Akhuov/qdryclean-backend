using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Charges.Quarries
{
    public class GetByIdChargeCommand : IRequest<ApiResponse<ChargeDto>>
    {
        public int Id { get; set; }
    }
}