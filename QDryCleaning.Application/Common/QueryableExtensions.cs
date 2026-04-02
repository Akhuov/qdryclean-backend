using Microsoft.EntityFrameworkCore;
using QDryClean.Domain;

namespace QDryClean.Application.Common.Pagination
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }


        public static PagedResult<T> ToPagedResult<T>(
            this IList<T> list,
            int page,
            int pageSize)
        {
            var totalCount = list.Count();

            var items = list
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> query)
        where T : Auditable
        {
            return query.Where(x => x.DeletedAt == null && x.DeletedBy == null);
        }
    }

}
