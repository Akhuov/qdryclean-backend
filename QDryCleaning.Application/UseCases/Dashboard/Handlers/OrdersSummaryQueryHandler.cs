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
            var from = DateTime.ParseExact(request.From, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;
            var to = DateTime.ParseExact(request.To, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date.AddDays(1);

            var ordersInPeriod = await _applicationDbContext.Orders
                .Include(o => o.Invoice)
                    .ThenInclude(i => i.Payments)
                .WhereNotDeleted()
                .Where(o => o.CreatedAt >= from && o.CreatedAt < to)
                .ToListAsync(cancellationToken);

            var paidAmount = ordersInPeriod
                .Where(o => o.Invoice.PaymentStatus == PaymentStatus.Paid)
                .Sum(o => o.Invoice.TotalCost);

            var unpaidAmount = ordersInPeriod
                .Where(o => o.Invoice.PaymentStatus == PaymentStatus.NotPaid)
                .Sum(o => o.Invoice.TotalCost);

            var partialOrders = ordersInPeriod
                .Where(o => o.Invoice.PaymentStatus == PaymentStatus.Partial);

            paidAmount += partialOrders.Sum(o => o.Invoice.Payments.Sum(p => p.Amount));

            unpaidAmount += partialOrders.Sum(o =>
                o.Invoice.TotalCost - o.Invoice.Payments.Sum(p => p.Amount));

            var activeOrders = await _applicationDbContext.Orders
                .WhereNotDeleted()
                .CountAsync(o => o.Status == OrderStatus.Created, cancellationToken);

            var readyOrders = await _applicationDbContext.Orders
                .WhereNotDeleted()
                .CountAsync(o => o.Status == OrderStatus.Ready, cancellationToken);

            var summary = new OrdersSummaryViewModel
            {
                ActiveOrders = activeOrders,
                ReadyOrders = readyOrders,
                Revenue = new PaymentRevenueViewModel
                {
                    Total = ordersInPeriod.Sum(o => o.Invoice.TotalCost),
                    Paid = paidAmount,
                    Unpaid = unpaidAmount,
                },
                TotalOrders = ordersInPeriod.Count,
                CompletedOrders = ordersInPeriod.Count(o => o.Status == OrderStatus.Completed),
            };

            return ApiResponseFactory.Ok(summary);
        }
    }
}
