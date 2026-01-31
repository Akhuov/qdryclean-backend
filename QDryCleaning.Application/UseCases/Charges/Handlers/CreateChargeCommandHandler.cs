using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Charges.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Charges.Handlers
{
    public class CreateChargeCommandHandler : CommandHandlerBase, IRequestHandler<CreateChargeCommand, ApiResponse<ChargeDto>>
    {
        public CreateChargeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<ChargeDto>> Handle(CreateChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = _mapper.Map<Charge>(request);
            charge.CreatedBy = _currentUserService.UserId;
            charge.CreatedAt = DateTime.Now;

            await _applicationDbContext.Charges.AddAsync(charge, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ChargeDto>(charge));
        }
    }
}