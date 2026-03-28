using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Helpers;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Customers.Commands;

namespace QDryClean.Application.UseCases.Customers.Handlers
{
    public class UpdateCustomerCommandHandler : CommandHandlerBase, IRequestHandler<UpdateCustomerCommand, ApiResponse<CustomerDto>>
    {
        public UpdateCustomerCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            customer.FullName = request.FirstName;
            customer.PhoneNumber = PhoneNumberHelper.NormalizePhoneNumber(request.PhoneNumber);
            customer.AdditionalPhoneNumber = PhoneNumberHelper.NormalizePhoneNumber(request.AdditionalPhoneNumber);
            customer.Points = request.Points;
            customer.UpdatedAt = DateTime.Now;
            customer.UpdatedBy = _currentUserService.UserId;

            _applicationDbContext.Customers.Update(customer);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<CustomerDto>(customer));
        }
    }
}
