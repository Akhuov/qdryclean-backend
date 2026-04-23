using QDryClean.Application.Dtos;
using QDryClean.Domain.Enums;

namespace QDryClean.Api.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int ReceiptNumber { get; set; }
        public CustomerDto Customer { get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public DateOnly CreatedAt { get; set; }
        public int ItemsCount { get; set; }
        public decimal TotalCost { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}