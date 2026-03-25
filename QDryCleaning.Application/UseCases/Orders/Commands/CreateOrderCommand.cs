using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Orders.Commands
{
    public class CreateOrderCommand : IRequest<ApiResponse<OrderDto>>
    {
        public DateOnly? ExpectedCompletionDate { get; set; } = null;
        public int CustomerId { get; set; }
        public string? Note { get; set; }
        public IList<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}