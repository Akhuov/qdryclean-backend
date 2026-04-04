using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos.Orders;

namespace QDryClean.Application.UseCases.Orders.Queries
{
    public class GetByIdOrderQuery : IRequest<ApiResponse<OrderDetailsDto>>
    {
        public int Id { get; set; }
    }
}
