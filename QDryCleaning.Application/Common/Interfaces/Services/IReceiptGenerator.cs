using QDryClean.Domain.Entities;

namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IReceiptGenerator
    {
        string GenerateEscPos(Order order);
    }
}