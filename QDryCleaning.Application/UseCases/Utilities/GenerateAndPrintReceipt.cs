using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Utilities
{
    public class GenerateAndPrintReceipt
    {
        private readonly IReceiptGenerator _generator;
        private readonly IPrinterService _printer;

        public GenerateAndPrintReceipt(
            IReceiptGenerator generator,
            IPrinterService printer)
        {
            _generator = generator;
            _printer = printer;
        }

        public async Task Execute(int invoiceId)
        {
            var escPosData = await _generator.GenerateEscPos(invoiceId);
            _printer.PrintRaw(escPosData);
        }
    }
}
