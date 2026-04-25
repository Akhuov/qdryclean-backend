using QDryClean.Domain.Enums;

namespace QDryClean.Application.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal AmountPaid { get; set; } = 0;
        public string? Notes { get; set; }
        public decimal Discount { get; set; }
    }
}
