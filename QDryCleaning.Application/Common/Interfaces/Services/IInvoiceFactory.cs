using QDryClean.Domain.Entities;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IInvoiceFactory
    {
        Invoice Create(Order order, IReadOnlyCollection<ItemType> itemTypes, PaymentStatus paymentStatus);
    }
}
