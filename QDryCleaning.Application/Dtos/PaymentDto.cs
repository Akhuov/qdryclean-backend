using QDryClean.Domain.Enums;

namespace QDryClean.Application.Dtos
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    }
}
