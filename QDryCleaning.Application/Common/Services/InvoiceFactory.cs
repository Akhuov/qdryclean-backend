using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Domain.Entities;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.Common.Services
{
    public class InvoiceFactory : IInvoiceFactory
    {
        public Invoice Create(Order order, IReadOnlyCollection<ItemType> itemTypes)
        {
            var invoiceItems = new List<Item>();
            decimal totalAmount = 0;

            foreach (var orderItem in order.Items)
            {
                var itemType = itemTypes.FirstOrDefault(x => x.Id == orderItem.ItemTypeId);

                if (itemType is null)
                    throw new NotFoundException($"ItemType with Id {orderItem.ItemTypeId} not found");
                totalAmount += itemType.Charge.Cost;
            }


            return new Invoice
            {
                Order = order,
                TotalCost = totalAmount,
                PaymentStatus = PaymentStatus.NotPaid,
            };
        }
    }
}
