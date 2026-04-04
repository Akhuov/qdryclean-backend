using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Domain.Entities;

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

        public async Task Execute(Invoice invoice)
        {
            var escPosData = await _generator.GenerateEscPos(invoice);
            _printer.PrintRaw(escPosData);
        }
    }
}
