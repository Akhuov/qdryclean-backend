using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Customers.Queries;

namespace QDryClean.Application.UseCases.Customers.Handlers
{
    public class GetAllCustomersQuerryHandler : CommandHandlerBase, IRequestHandler<GetAllCustomersQuery, ApiResponse<PagedResult<CustomerDto>>>
    {
        public GetAllCustomersQuerryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<PagedResult<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _applicationDbContext.Customers
                .Where(x => x.DeletedAt == null && x.DeletedBy == null)
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider);

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
