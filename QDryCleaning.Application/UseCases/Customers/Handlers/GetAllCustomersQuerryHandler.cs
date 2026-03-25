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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var query = _applicationDbContext.Customers
                .AsNoTracking()
                .Where(x => x.DeletedAt == null && x.DeletedBy == null)
                .AsQueryable();

            var pagedResult = await query
                .OrderBy(c => c.Id)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToPagedResultAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken
                );

            return ApiResponseFactory.Ok(pagedResult);
        }
    }
}
