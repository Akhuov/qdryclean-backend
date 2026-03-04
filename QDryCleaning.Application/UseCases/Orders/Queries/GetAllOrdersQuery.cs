using MediatR;
using QDryClean.Api.ViewModels;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;

namespace QDryClean.Application.UseCases.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<ApiResponse<PagedResult<OrderViewModel>>>, IPagedQuery
    {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
        public string? Search { get; set; }
    }
}
