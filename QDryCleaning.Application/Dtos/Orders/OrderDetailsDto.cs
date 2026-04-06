using QDryClean.Application.ViewModels;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.Dtos.Orders
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public CustomerDto Customer { get; set; }
        public int ReceiptNumber { get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public DateOnly CreatedAt { get; set; }
        public List<ItemViewModel> Items { get; set; }
        public IList<string> Notes { get; set; } = new List<string>();
    }
}
