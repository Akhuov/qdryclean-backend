using MediatR;
using QDryClean.Application.Dtos;
using QDryClean.Domain.Enums;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Invoices.Commands
{
    public class UpdateInvoiceCommand : IRequest<InvoiceDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Notes { get; set; }
        public decimal Discount { get; set; }
        public int OrderId { get; set; }
    }
}
