using QDryClean.Domain.Entities;

namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IInvoiceFactory
    {
        Invoice Create(Order order, IReadOnlyCollection<ItemType> itemTypes);
    }
}
