using QDryClean.Domain.Enums;

namespace QDryClean.Domain.Entities
{
    public class Invoice : BaseModel
    {
        public decimal TotalCost { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
        public decimal Discount { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
