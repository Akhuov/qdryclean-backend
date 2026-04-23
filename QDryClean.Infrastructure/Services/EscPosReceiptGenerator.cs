using QDryClean.Application.Common.Helpers;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Domain.Entities;
using QDryClean.Domain.Enums;
using System.Text;

namespace QDryClean.Infrastructure.Services
{
    public class EscPosReceiptGenerator : IReceiptGenerator
    {
        private const int LineWidth = 48; // 80mm
        private const int CodeTable = 17;

        public string GenerateEscPos(Order order)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = new List<byte>();
            var encoding = Encoding.GetEncoding(866);

            void Write(string text) => builder.AddRange(encoding.GetBytes(text));
            void Cmd(params byte[] cmd) => builder.AddRange(cmd);
            void Line(string text = "") => Write(text + "\n");

            string Divider(char c = '-') => new string(c, LineWidth);

            string Center(string text)
            {
                text ??= string.Empty;
                if (text.Length >= LineWidth) return text;

                int pad = (LineWidth - text.Length) / 2;
                return new string(' ', pad) + text;
            }

            string Fit(string text, int maxLength)
            {
                text ??= string.Empty;

                if (text.Length <= maxLength)
                    return text;

                if (maxLength <= 3)
                    return text[..maxLength];

                return text[..(maxLength - 3)] + "...";
            }

            string LeftRight(string left, string right)
            {
                left ??= string.Empty;
                right ??= string.Empty;

                if (left.Length + right.Length >= LineWidth)
                {
                    left = Fit(left, Math.Max(1, LineWidth - right.Length - 1));
                }

                int spaces = Math.Max(1, LineWidth - left.Length - right.Length);
                return left + new string(' ', spaces) + right;
            }

            void WriteWrappedLeftRight(string left, string right)
            {
                left ??= string.Empty;
                right ??= string.Empty;

                int maxLeftWidth = Math.Max(1, LineWidth - right.Length - 1);

                if (left.Length <= maxLeftWidth)
                {
                    Line(LeftRight(left, right));
                    return;
                }

                while (left.Length > maxLeftWidth)
                {
                    Line(left[..maxLeftWidth]);
                    left = left[maxLeftWidth..];
                }

                Line(LeftRight(left, right));
            }

            string Money(decimal value) => $"{value:0}";
            string FormatDate(DateOnly value) => value.ToString("dd.MM.yyyy");
            string FormatDateTime(DateTime value) => value.ToString("dd.MM.yyyy");

            string StatusToText(OrderStatus status) => status switch
            {
                OrderStatus.Draft => "Заказ Создан",
                OrderStatus.Created => "Заказ Оформлен",
                _ => "Неизвестно"
            };

            string PaymentStatusToText(PaymentStatus status) => status switch
            {
                PaymentStatus.NotPaid => "Не оплачен",
                PaymentStatus.Paid => "Оплачен",
                PaymentStatus.Partial => "Частично оплачен",
                _ => "Неизвестно"
            };

            string Safe(string? value, string fallback = "Не указан")
                => string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

            decimal itemsTotal = order.Items?.Sum(x => x.ItemType?.Charge?.Cost ?? 0) ?? 0;
            decimal totalCost = order.Invoice?.TotalCost ?? itemsTotal;
            decimal discount = order.Invoice?.Discount ?? 0;
            string paymentStatus = PaymentStatusToText(order.Invoice?.PaymentStatus ?? 0);

            var customerName = Safe(order.Customer?.FullName);
            var customerPhone = Safe(order.Customer?.PhoneNumber);
            var createdAt = FormatDateTime(order.CreatedAt);
            var expectedDate = FormatDate(order.ExpectedCompletionDate);

            // INIT
            Cmd(0x1B, 0x40); // init
            Cmd(0x1C, 0x2E); // disable Chinese mode
            Cmd(0x1B, 0x74, CodeTable); // CP866

            // 80 mm print area
            Cmd(0x1D, 0x4C, 0x00, 0x00); // left margin = 0
            Cmd(0x1D, 0x57, 0x40, 0x02); // width = 576 dots

            // Header
            Cmd(0x1B, 0x61, 0x01); // center
            Cmd(0x1B, 0x45, 0x01); // bold on
            Line("Химчистка LUXE");
            Cmd(0x1B, 0x45, 0x00); // bold off

            Line("Тел: +998 XX XXX XX XX");
            Line("Адрес: Tashkent");
            Line(Divider());

            // Order info
            Cmd(0x1B, 0x61, 0x00); // left

            Line(LeftRight($"Чек №: {order.ReceiptNumber}", $"Дата: {createdAt}"));
            Line($"Клиент: {customerName}");
            Line($"Тел: {customerPhone}");
            Line(LeftRight("Статус:", StatusToText(order.Status)));
            Line(LeftRight("Готово до:", expectedDate));

            Line(Divider());

            // Items
            Cmd(0x1B, 0x45, 0x01); // bold on
            Line(LeftRight("Наименование", "Цена"));
            Cmd(0x1B, 0x45, 0x00); // bold off
            Line(Divider());

            if (order.Items != null && order.Items.Any())
            {
                foreach (var item in order.Items)
                {
                    var typeName = Safe(item.ItemType?.Name, "Услуга");
                    var brand = Safe(item.BrandName, "Не указан");
                    var color = Safe(item.Colour, "Не указан");
                    var description = Safe(item.Description, "Нет заметок");
                    var price = item.ItemType?.Charge?.Cost ?? 0;

                    WriteWrappedLeftRight(typeName, Money(price));

                    if (!string.Equals(brand, "Не указан", StringComparison.OrdinalIgnoreCase))
                        Line($"  Бренд: {brand}");

                    if (!string.Equals(color, "Не указан", StringComparison.OrdinalIgnoreCase))
                        Line($"  Цвет: {color}");

                    if (!string.Equals(description, "Нет заметок", StringComparison.OrdinalIgnoreCase))
                        Line($"  Описание: {description}");

                    Line();
                }
            }
            else
            {
                Line("Нет позиций");
            }

            Line(Divider());

            // Payment
            Line(LeftRight("Оплата:", paymentStatus));

            if (discount > 0)
                Line(LeftRight("Скидка:", Money(discount)));

            Cmd(0x1B, 0x45, 0x01); // bold on
            Line(LeftRight("ИТОГО:", Money(totalCost)));
            Cmd(0x1B, 0x45, 0x00); // bold off

            // Notes
            if (order.Notes != null && order.Notes.Any())
            {
                Line(Divider());
                Cmd(0x1B, 0x45, 0x01);
                Line("Заметки:");
                Cmd(0x1B, 0x45, 0x00);

                foreach (var note in order.Notes.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    Line($"- {note}");
                }
            }

            Line();
            Cmd(0x1B, 0x61, 0x01); // center
            Line("Спасибо за визит!");
            Line();

            // Native QR
            builder.AddRange(EscPosQrHelper.GenerateQrCommands(
                order.ReceiptNumber.ToString(),
                moduleSize: 10,
                errorCorrection: 49
            ));
            Line();
            Line();


            Line($"Заказ № {order.ReceiptNumber}");

            Line();
            Line();
            Line();
            Line();
            Line();


            Cmd(0x1D, 0x56, 0x00); // cut

            return Convert.ToBase64String(builder.ToArray());
        }
    }
}