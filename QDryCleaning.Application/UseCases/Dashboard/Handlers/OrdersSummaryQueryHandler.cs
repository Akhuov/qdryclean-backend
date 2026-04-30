using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Helpers;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Dashboard.Queries;
using QDryClean.Application.ViewModels.Dashboard;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Dashboard.Handlers
{
    public class OrdersSummaryQueryHandler : BaseHandler, IRequestHandler<OrdersSummaryQuery, ApiResponse<OrdersSummaryViewModel>>
    {
        public OrdersSummaryQueryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<OrdersSummaryViewModel>> Handle(
            OrdersSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var (fromDate, toDate, error) = DateRangeParser.Parse(request.From, request.To);

            if (error != null)
                return ApiResponseFactory.Fail<OrdersSummaryViewModel>(400, error);

            var baseQuery = _applicationDbContext.Orders
                .AsNoTracking()
                .WhereNotDeleted();

            var periodQuery = baseQuery
                .ApplyDateRange(fromDate, toDate); // ✅ reuse

            // ---------------------------
            // Revenue (минимальный select)
            // ---------------------------
            var revenueData = await periodQuery
                .Select(o => new
                {
                    o.Status,
                    o.Invoice.TotalCost,
                    o.Invoice.PaymentStatus,
                    PaidAmount = o.Invoice.Payments.Sum(p => (decimal?)p.Amount) ?? 0
                })
                .ToListAsync(cancellationToken);

            var total = revenueData.Sum(x => x.TotalCost);

            var paid = revenueData
                .Where(x => x.PaymentStatus == PaymentStatus.Paid)
                .Sum(x => x.TotalCost);

            var partialPaid = revenueData
                .Where(x => x.PaymentStatus == PaymentStatus.Partial)
                .Sum(x => x.PaidAmount);

            var partialUnpaid = revenueData
                .Where(x => x.PaymentStatus == PaymentStatus.Partial)
                .Sum(x => x.TotalCost - x.PaidAmount);

            var unpaid = revenueData
                .Where(x => x.PaymentStatus == PaymentStatus.NotPaid)
                .Sum(x => x.TotalCost);

            paid += partialPaid;
            unpaid += partialUnpaid;

            // ---------------------------
            // Status counts (reuse baseQuery)
            // ---------------------------
            var statusCounts = await baseQuery
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var activeOrders = statusCounts
                .FirstOrDefault(x => x.Status == OrderStatus.Created)?.Count ?? 0;

            var readyOrders = statusCounts
                .FirstOrDefault(x => x.Status == OrderStatus.Ready)?.Count ?? 0;

            var completedOrders = revenueData
                .Count(x => x.Status == OrderStatus.Completed);

            var summary = new OrdersSummaryViewModel
            {
                ActiveOrders = activeOrders,
                ReadyOrders = readyOrders,
                TotalOrders = revenueData.Count,
                CompletedOrders = completedOrders,
                Revenue = new PaymentRevenueViewModel
                {
                    Total = total,
                    Paid = paid,
                    Unpaid = unpaid
                }
            };

            return ApiResponseFactory.Ok(summary);
        }
    }
}
