using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Api.ViewModels;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Orders.Queries;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetAllOrdersCommandHandler : CommandHandlerBase, IRequestHandler<GetAllOrdersQuery, ApiResponse<PagedResult<OrderViewModel>>>
    {
        public GetAllOrdersCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<PagedResult<OrderViewModel>>> Handle(
            GetAllOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.Orders
                .AsNoTracking()
                .Where(x => x.DeletedAt == null && x.DeletedBy == null)
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim();
                var isNumber = int.TryParse(s, out var n);

                // Для SQL Server Like нечувствителен к регистру при CI collation (обычно так и есть)
                var like = $"%{s}%";

                query = query.Where(o =>
                    (isNumber && (o.Id == n || o.ReceiptNumber == n)) ||
                    o.Customer.FirstName != null && EF.Functions.Like(o.Customer.FirstName, like) ||
                    o.Customer.LastName != null && EF.Functions.Like(o.Customer.LastName, like) ||
                    o.Notes.Any(note => EF.Functions.Like(note, like))
                );
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(o => o.ReceiptNumber)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    CustomerName = (o.Customer.LastName + " " ?? "") + (o.Customer.FirstName ?? ""),
                    ReceiptNumber = o.ReceiptNumber,
                    ProcessStatus = o.ProcessStatus,
                    ExpectedCompletionDate = o.ExpectedCompletionDate,
                    CreatedAt = DateOnly.FromDateTime(o.CreatedAt),
                    ItemsCount = o.Items.Count(),
                    Notes = o.Notes
                })
                .ToPagedResultAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken);

            return ApiResponseFactory.Ok(items);
        }
    }
}