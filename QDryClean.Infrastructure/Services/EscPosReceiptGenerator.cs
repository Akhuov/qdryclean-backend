using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Helpers;
using QDryClean.Application.Common.Interfaces.Services;
using System.Text;

namespace QDryClean.Infrastructure.Services
{
    public class EscPosReceiptGenerator : IReceiptGenerator
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _dbContext;
        private readonly IQrCodeService _qrService;
        public EscPosReceiptGenerator(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IQrCodeService qrService)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _qrService = qrService;
        }
        public async Task<byte[]> GenerateEscPos(int invoiceId)
        {
            var cashier = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);
            var cashierName = cashier != null ? $"{cashier.FirstName} {cashier.LastName}" : "Unknown";
            var invoiceWithOrderItems = await _dbContext.OrderInvoices
                .AsNoTracking()
                .Where(i => i.Id == invoiceId)
                // Подгружаем заказ и его товары
                .Include(i => i.Order)
                    .ThenInclude(o => o.Items)
                // Подгружаем тип товара и его Charge
                .Include(i => i.Order)
                    .ThenInclude(o => o.Items)
                        .ThenInclude(oi => oi.ItemType)
                            .ThenInclude(it => it.Charge)
                .FirstOrDefaultAsync();

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
            Write($"Кассир: {cashierName}\n");
            Write("--------------------------------\n");

            foreach (var item in invoiceWithOrderItems.Order.Items)
            {
                Write($"{item.BrandName}\n");
                Write($"{item.ItemType.Charge.Cost}\n");
            }

            Write("--------------------------------\n");

            // Жирный текст
            Command(0x1B, 0x45, 0x01);
            Write($"ИТОГО: {invoiceWithOrderItems.TotalCost}\n");
            Command(0x1B, 0x45, 0x00);

            Write("\n");

            // QR-код
            var qrBytes = _qrService.GenerateQrCode($"{invoiceWithOrderItems.Order.ReceiptNumber}"); // или другой контент
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
