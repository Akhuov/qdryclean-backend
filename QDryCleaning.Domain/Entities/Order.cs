using QDryClean.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace QDryClean.Domain.Entities
{
    public class Order : Auditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptNumber { get; set; }
        public OrderStatus Status { get; set; }
        public DateOnly ExpectedCompletionDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Invoice Invoice { get; set; }
        public IList<string> Notes { get; set; } = new List<string>();
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
