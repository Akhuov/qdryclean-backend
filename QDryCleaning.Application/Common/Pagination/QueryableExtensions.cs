using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace QDryClean.Application.Common.Pagination
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
            this IQueryable<TSource> query,
            int page,
            int pageSize,
            Expression<Func<TSource, TResult>> selector,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return new PagedResult<TResult>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }

}
