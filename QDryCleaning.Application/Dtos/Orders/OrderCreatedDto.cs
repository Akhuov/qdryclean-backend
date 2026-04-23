using QDryClean.Domain.Enums;

namespace QDryClean.Application.Dtos.Orders
{
    public class OrderCreatedDto
    {
        public int Id { get; set; }
        public int ReceiptNumber { get; set; }
        public CustomerDto Customer { get; set; }
        public InvoiceDto Invoice { get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public ICollection<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
