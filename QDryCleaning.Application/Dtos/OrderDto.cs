
using QDryClean.Domain.Enums;

namespace QDryClean.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int customerId { get; set; }
        public int ReceiptNumber { get; set; }
        public ProcessStatus ProcessStatus { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public IList<string> Notes { get; set; } = new List<string>();

        public ICollection<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
