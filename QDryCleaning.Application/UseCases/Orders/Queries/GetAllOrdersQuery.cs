using MediatR;
using QDryClean.Api.ViewModels;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<ApiResponse<PagedResult<OrderViewModel>>>, IPagedQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Search { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
