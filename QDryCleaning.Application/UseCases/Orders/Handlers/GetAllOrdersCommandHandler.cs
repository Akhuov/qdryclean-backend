using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Orders.Queries;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetAllOrdersCommandHandler : CommandHandlerBase, IRequestHandler<GetAllOrdersQuery, ApiResponse<PagedResult<OrderDto>>>
    {
        public GetAllOrdersCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<PagedResult<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _applicationDbContext.Orders
                .Where(x => x.DeletedAt == null && x.DeletedBy == null)
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            var pagedResult = await baseQuery
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .ToPagedResultAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken
                );

            return ApiResponseFactory.Ok(pagedResult);
        }
    }
}