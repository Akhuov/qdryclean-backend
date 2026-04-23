using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Api.ViewModels;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Orders.Queries;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetAllOrdersQueryHandler : BaseHandler, IRequestHandler<GetAllOrdersQuery, ApiResponse<PagedResult<OrderViewModel>>>
    {
        public GetAllOrdersQueryHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<PagedResult<OrderViewModel>>> Handle(
            GetAllOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.Orders
                .Include(o => o.Invoice)
                .AsNoTracking()
                .WhereNotDeleted()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim();
                var isNumber = int.TryParse(s, out var n);

                // Для SQL Server Like нечувствителен к регистру при CI collation (обычно так и есть)
                var like = $"%{s}%";

                query = query.Where(o =>
                    (isNumber && (o.Id == n || o.ReceiptNumber == n)) ||
                    o.Customer.FullName != null && EF.Functions.Like(o.Customer.FullName, like) ||
                    o.Notes.Any(note => EF.Functions.Like(note, like))
                );
            }

            if (request.Status.HasValue)
            {
                query = query.Where(o => o.Status == request.Status);
            }

            var items = await query
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Customer = new CustomerDto
                    {
                        Id = o.Customer.Id,
                        FullName = o.Customer.FullName,
                        PhoneNumber = o.Customer.PhoneNumber,
                        AdditionalPhoneNumber = o.Customer.AdditionalPhoneNumber
                    },
                    ReceiptNumber = o.ReceiptNumber,
                    Status = o.Status,
                    ExpectedCompletionDate = o.ExpectedCompletionDate,
                    CreatedAt = DateOnly.FromDateTime(o.CreatedAt),
                    TotalCost = o.Invoice.TotalCost,
                    ItemsCount = o.Items.Count(),
                    PaymentStatus = o.Invoice.PaymentStatus
                })
                .ToPagedResultAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken);

            return ApiResponseFactory.Ok(items);
        }
    }
}