using QDryClean.Application.Dtos;

namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IReceiptGenerator
    {
        Task<byte[]> GenerateEscPos(int invoiceId);
    }
}