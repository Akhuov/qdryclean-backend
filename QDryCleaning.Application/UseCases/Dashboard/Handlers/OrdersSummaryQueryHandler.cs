using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Dashboard.Queries;
using QDryClean.Application.ViewModels.Dashboard;
using QDryClean.Domain.Enums;
using System.Globalization;

namespace QDryClean.Application.UseCases.Dashboard.Handlers
{
    public class OrdersSummaryQueryHandler : BaseHandler, IRequestHandler<OrdersSummaryQuery, ApiResponse<OrdersSummaryViewModel>>
    {
        public OrdersSummaryQueryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<OrdersSummaryViewModel>> Handle(OrdersSummaryQuery request, CancellationToken cancellationToken)
        {
            var fromParsed = DateTime.ParseExact(
                request.From,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);

            var toParsed = DateTime.ParseExact(
                request.To,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);

            var from = fromParsed.Date;
            var to = toParsed.Date.AddDays(1);

            var all_orders = await _applicationDbContext.Orders
                .Include(o => o.Invoice)
                .WhereNotDeleted()
                .ToListAsync();

            var orders_in_period = all_orders.Where(o => o.CreatedAt >= from && o.CreatedAt < to).ToList();

            var summary = new OrdersSummaryViewModel 
            {
                // Active orders are those that are created but not yet completed or canceled
                ActiveOrders = all_orders.Count(o => o.Status == OrderStatus.Created),
                ReadyOrders = all_orders.Count(o => o.Status == OrderStatus.Ready),

                // Total revenue is calculated based on the invoice total cost, categorized by payment status
                Revenue = new PaymentRevenueViewModel
                {
                    Total = orders_in_period.Sum(o => o.Invoice.TotalCost),
                    Paid = orders_in_period.Where(o => o.Invoice.PaymentStatus == PaymentStatus.Paid).Sum(o => o.Invoice.TotalCost),
                    Unpaid = orders_in_period.Where(o => o.Invoice.PaymentStatus == PaymentStatus.NotPaid).Sum(o => o.Invoice.TotalCost),
                    PartiallyPaid = orders_in_period.Where(o => o.Invoice.PaymentStatus == PaymentStatus.Partial).Sum(o => o.Invoice.TotalCost)
                },
                TotalOrders = orders_in_period.Count(),
                CompletedOrders = orders_in_period.Count(o => o.Status == OrderStatus.Completed),
            };

            return ApiResponseFactory.Ok(summary);
        }
    }
}
