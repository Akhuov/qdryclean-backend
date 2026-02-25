namespace QDryClean.Application.Common.Interfaces.Services
{
    public interface IQrCodeService
    {
        byte[] GenerateQrCode(string content);
        string GenerateQrCodeBase64(string content);
    }
}
