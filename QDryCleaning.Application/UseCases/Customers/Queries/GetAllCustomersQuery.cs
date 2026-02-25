using MediatR;
using QDryClean.Application.Common.Interfaces;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<ApiResponse<PagedResult<CustomerDto>>> , IPagedQuery
    {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}