using Microsoft.EntityFrameworkCore;
using QDryClean.Domain.Entities;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.Common.Helpers
{
    public static class OrderQueryFilters
    {
        public static IQueryable<Order> ApplySearch(
            this IQueryable<Order> query,
            string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return query;

            var s = search.Trim();
            var like = $"%{s}%";
            var isNumber = int.TryParse(s, out var n);

            return query.Where(o =>
                (isNumber && (o.Id == n || o.ReceiptNumber == n)) ||
                (o.Customer.FullName != null && EF.Functions.Like(o.Customer.FullName, like)) ||
                o.Notes.Any(note => EF.Functions.Like(note, like))
            );
        }

        public static IQueryable<Order> ApplyStatus(
            this IQueryable<Order> query,
            OrderStatus? status)
        {
            if (status.HasValue)
                return query.Where(o => o.Status == status.Value);

            return query;
        }

        public static IQueryable<Order> ApplyDateRange(
            this IQueryable<Order> query,
            DateTime? from,
            DateTime? to)
        {
            if (from.HasValue)
                query = query.Where(o => o.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(o => o.CreatedAt <= to.Value);

            return query;
        }
    }
}
