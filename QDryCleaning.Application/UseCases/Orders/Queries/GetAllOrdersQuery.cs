using MediatR;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<ApiResponse<PagedResult<OrderDto>>>, IPagedQuery
    {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
