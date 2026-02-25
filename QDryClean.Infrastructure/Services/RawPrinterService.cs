using QDryClean.Application.Common.Helpers;
using QDryClean.Application.Common.Interfaces.Services;

namespace QDryClean.Infrastructure.Services
{
    public class RawPrinterService : IPrinterService
    {
        private readonly string _printerName;

        public RawPrinterService(string printerName)
        {
            _printerName = printerName;
        }

        public void PrintRaw(byte[] data)
        {
            RawPrinterHelper.SendBytesToPrinter(_printerName, data);
        }
    }
}
