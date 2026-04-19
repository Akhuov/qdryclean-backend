using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.Dtos.Orders;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Orders.Commands
{
    public class CreateOrderCommand : IRequest<ApiResponse<OrderCreatedDto>>
    {
        public DateOnly? ExpectedCompletionDate { get; set; } = null;
        public int CustomerId { get; set; }
        public string? Note { get; set; }
        public IList<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
        public decimal PaidAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}