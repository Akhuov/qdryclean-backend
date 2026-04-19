using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Customers.Commands;

namespace QDryClean.Application.UseCases.Customers.Handlers
{
    public class SoftDeleteCustomerCommandHandler : BaseHandler, IRequestHandler<SoftDeleteCustomerCommand, ApiResponse<Unit>>
    {

        public SoftDeleteCustomerCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(SoftDeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            customer.DeletedAt = DateTime.UtcNow;
            customer.DeletedBy = _currentUserService.UserId;

            _applicationDbContext.Customers.Update(customer);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
