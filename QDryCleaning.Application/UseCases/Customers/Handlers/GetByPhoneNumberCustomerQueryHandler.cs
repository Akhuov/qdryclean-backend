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
    public class GetByPhoneNumberCustomerQueryHandler : BaseHandler, IRequestHandler<GetByPhoneNumberCustomerQuery, ApiResponse<CustomerDto>>
    {
        public GetByPhoneNumberCustomerQueryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<CustomerDto>> Handle(GetByPhoneNumberCustomerQuery request, CancellationToken cancellationToken)
        {
            var normalizedPhone = $"+998{request.PhoneNumber}";
            var customer = await _applicationDbContext.Customers
                    .FirstOrDefaultAsync(
                        x => x.PhoneNumber == normalizedPhone &&
                             x.DeletedAt == null &&
                             x.DeletedBy == null,
                        cancellationToken);

            return customer == null
                ? throw new NotFoundException("Customer with this phone number does not exist")
                : ApiResponseFactory.Ok(_mapper.Map<CustomerDto>(customer));
        }
    }
}