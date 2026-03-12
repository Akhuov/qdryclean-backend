using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Customers.Queries;
using System.Numerics;

namespace QDryClean.Application.UseCases.Customers.Handlers
{
    public class GetByPhoneNumberCustomerQueryHandler : CommandHandlerBase, IRequestHandler<GetByPhoneNumberCustomerQuery, ApiResponse<CustomerDto>>
    {
        public GetByPhoneNumberCustomerQueryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<CustomerDto>> Handle(GetByPhoneNumberCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.Customers
                    .FirstOrDefaultAsync(
                        x => x.PhoneNumber == request.PhoneNumber &&
                             x.DeletedAt == null &&
                             x.DeletedBy == null,
                        cancellationToken);

            return ApiResponseFactory.Ok(_mapper.Map<CustomerDto>(customer));
        }
    }
}