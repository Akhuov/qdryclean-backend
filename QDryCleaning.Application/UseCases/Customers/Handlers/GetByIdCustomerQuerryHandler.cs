using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Customers.Queries;

namespace QDryClean.Application.UseCases.Customers.Handlers
{
    public class GetByIdCustomerQuerryHandler : BaseHandler, IRequestHandler<GetByIdCustomerQuery, ApiResponse<CustomerDto>>
    {
        public GetByIdCustomerQuerryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<CustomerDto>> Handle(GetByIdCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return customer == null
                ? throw new NotFoundException("Customer with this ID does not exist")
                : ApiResponseFactory.Ok(_mapper.Map<CustomerDto>(customer));
        }
    }
}