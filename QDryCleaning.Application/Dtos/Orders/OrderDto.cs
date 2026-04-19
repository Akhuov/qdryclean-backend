using QDryClean.Domain.Enums;

namespace QDryClean.Application.Dtos.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ReceiptNumber { get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public IList<string> Notes { get; set; } = new List<string>();

        public ICollection<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
