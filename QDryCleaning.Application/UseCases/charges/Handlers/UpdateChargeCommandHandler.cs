using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Charges.Commands;

namespace QDryClean.Application.UseCases.Charges.Handlers
{
    public class UpdateChargeCommandHandler : BaseHandler, IRequestHandler<UpdateChargeCommand, ApiResponse<ChargeDto>>
    {
        public UpdateChargeCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ChargeDto>> Handle(UpdateChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _applicationDbContext.Charges.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (request.Cost != null)
            {
                charge.Cost = request.Cost;
            }
            if (request.Name != null)
            {
                charge.Name = request.Name;
            }

            charge.UpdatedAt = DateTime.UtcNow;
            charge.UpdatedBy = _currentUserService.UserId;

            _applicationDbContext.Charges.Update(charge);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ChargeDto>(charge));
        }
    }
}
