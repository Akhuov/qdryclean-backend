using QDryClean.Domain.Entities;

namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IReceiptGenerator
    {
        Task<byte[]> GenerateEscPos(Invoice invocie);
    }
}