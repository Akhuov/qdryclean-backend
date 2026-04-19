using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Customers.Commands;

namespace QDryClean.Application.UseCases.Customers.Handlers
{
    public class DeleteCustomerCommandHandler : BaseHandler, IRequestHandler<DeleteCustomerCommand, ApiResponse<Unit>>
    {
        public DeleteCustomerCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            _applicationDbContext.Customers.Remove(customer);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}

