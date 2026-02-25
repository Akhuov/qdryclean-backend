using QDryClean.Application.Common.Interfaces;
using QRCoder;

namespace QDryClean.Infrastructure.Services
{
    public class QrCodeService : IQrCodeService
    {
        public byte[] GenerateQrCode(string content)
        {
            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(data);

            return qrCode.GetGraphic(20);
        }

        public string GenerateQrCodeBase64(string content)
        {
            var bytes = GenerateQrCode(content);
            return Convert.ToBase64String(bytes);
        }
    }
}
