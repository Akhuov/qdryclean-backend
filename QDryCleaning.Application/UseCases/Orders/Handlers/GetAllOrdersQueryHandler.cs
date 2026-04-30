using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Api.ViewModels;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Helpers;
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
            // 1. Parse dates
            var (fromDate, toDate, error) = DateRangeParser.Parse(request.From, request.To);
            if (error != null)
                return ApiResponseFactory.Fail<PagedResult<OrderViewModel>>(400, error);

            // 2. Base query
            var query = _applicationDbContext.Orders
                .AsNoTracking()
                .WhereNotDeleted()
                .ApplyStatus(request.Status)
                .ApplyDateRange(fromDate, toDate)
                .ApplySearch(request.Search);


            // 3. Projection + pagination
            var result = await query
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
                .ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);

            return ApiResponseFactory.Ok(result);
        }
    }
}