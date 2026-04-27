using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Domain.Enums;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Payments.Commands
{
    public class CreatePaymentByOrderIdCommand : IRequest<ApiResponse<Unit>>
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
