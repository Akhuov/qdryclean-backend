using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Charges.Quarries;

namespace QDryClean.Application.UseCases.Charges.Handlers
{
    public class GetByIdChargeCommandHandler : BaseHandler, IRequestHandler<GetByIdChargeCommand, ApiResponse<ChargeDto>>
    {
        public GetByIdChargeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ChargeDto>> Handle(GetByIdChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _applicationDbContext.Charges.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ChargeDto>(charge));
        }
    }
}
