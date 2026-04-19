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
    public class GetAllChargesCommandHandler : BaseHandler, IRequestHandler<GetAllChargesCommand, ApiResponse<List<ChargeDto>>>
    {
        public GetAllChargesCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<List<ChargeDto>>> Handle(GetAllChargesCommand request, CancellationToken cancellationToken)
        {

            var charges = await _applicationDbContext.Charges.ToListAsync();

            var listOfChargesDtos = new List<ChargeDto>();
            foreach (var charge in charges)
            {
                listOfChargesDtos.Add(_mapper.Map<ChargeDto>(charge));
            }

            return ApiResponseFactory.Ok(listOfChargesDtos);
        }
    }
}
