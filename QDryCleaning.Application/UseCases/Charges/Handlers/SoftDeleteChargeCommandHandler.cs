using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Charges.Commands;

namespace QDryClean.Application.UseCases.Charges.Handlers
{
    public class SoftDeleteChargeCommandHandler : BaseHandler, IRequestHandler<SoftDeleteChargeCommand, ApiResponse<Unit>>
    {
        public SoftDeleteChargeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(SoftDeleteChargeCommand request, CancellationToken cancellationToken)
        {

            var charge = await _applicationDbContext.Charges.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            _applicationDbContext.Charges.Remove(charge);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
