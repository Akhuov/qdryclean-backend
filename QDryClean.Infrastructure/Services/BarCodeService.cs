using QDryClean.Application.Common.Interfaces.Services;
using System.Text;
using ZXing;
using ZXing.Common;


namespace QDryClean.Infrastructure.Services
{
    public class BarCodeService : IBarCodeService
    {
        public byte[] GenerateCode128(string content)
        {
            var writer = new BarcodeWriterSvg
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = 400,
                    Height = 150,
                    Margin = 10
                }
            };

            var svg = writer.Write(content);

            return Encoding.UTF8.GetBytes(svg.Content);
        }
    }
}
