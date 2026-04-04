using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Helpers;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Domain.Entities;
using System.Text;

namespace QDryClean.Infrastructure.Services
{
    public class EscPosReceiptGenerator : IReceiptGenerator
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IQrCodeService _qrService;
        public EscPosReceiptGenerator(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IQrCodeService qrService)
        {
            _currentUserService = currentUserService;
            _qrService = qrService;
        }
        public async Task<byte[]> GenerateEscPos(Invoice invoice)
        {
            var cashier = _currentUserService.UserId;

            var builder = new List<byte>();

            void Write(string text)
            {
                builder.AddRange(Encoding.UTF8.GetBytes(text));
            }

            void Command(params byte[] cmd)
            {
                builder.AddRange(cmd);
            }

            // Инициализация
            Command(0x1B, 0x40); // ESC @

            // Центрирование
            Command(0x1B, 0x61, 0x01);
            Write("Химчистка LUXE" + "\n");

            // Обычное выравнивание
            Command(0x1B, 0x61, 0x00);
            Write($"Адрес: Test Address\n");
            Write($"Дата: {DateTime.UtcNow:dd.MM.yyyy HH:mm}\n");
            Write($"Кассир: {cashier}\n");
            Write("--------------------------------\n");

            foreach (var item in invoice.Order.Items)
            {
                Write($"{item.BrandName}\n");
                Write($"{item.ItemType.Charge.Cost}\n");
            }

            Write("--------------------------------\n");

            // Жирный текст
            Command(0x1B, 0x45, 0x01);
            Write($"ИТОГО: {invoice.TotalCost}\n");
            Command(0x1B, 0x45, 0x00);

            Write("\n");

            // QR-код
            var qrBytes = _qrService.GenerateQrCode($"{invoice.Order.ReceiptNumber}"); // или другой контент

            var escPosQr = EscPosQrHelper.ConvertQrToEscPos(qrBytes);
            builder.AddRange(escPosQr);
            Command(0x1B, 0x61, 0x00);
            Write($"Reseipt Number\n");
            Write("\n\n\n");

            // Обрезка бумаги
            Command(0x1D, 0x56, 0x00);

            return builder.ToArray();
        }
    }
}
