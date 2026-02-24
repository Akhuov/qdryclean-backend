using AutoMapper;
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
            .AsNoTracking().Where(x => x.DeletedAt == null 
                && x.DeletedBy == null);

            var pagedResult = await baseQuery
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .ToPagedResultAsync(
                    request.Page,
                    request.PageSize,
                    c => new CustomerDto
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PhoneNumber = c.PhoneNumber,
                        AdditionalPhoneNumber = c.AdditionalPhoneNumber,
                        Points = c.Points
                    },
                    cancellationToken
                );

            return ApiResponseFactory.Ok(pagedResult);
        }
    }
}
