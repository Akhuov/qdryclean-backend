using MediatR;
using QDryClean.Application.Dtos;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Invoices.Commands
{
    public class CreateInvoiceCommand : IRequest<InvoiceDto>
    {
        public required decimal TotalCost { get; set; }
        public required PaymentStatus PaymentStatus { get; set; } = PaymentStatus.NotPaid;
        public string? Notes { get; set; }
        public decimal Discount { get; set; }
        public int OrderId { get; set; }
    }
}
