using QDryClean.Domain.Enums;

namespace QDryClean.Domain.Entities
{
    public class Payment : Auditable
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? Notes { get; set; }
    }
}
