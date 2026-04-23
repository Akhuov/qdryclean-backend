using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Orders.Commands;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class PatchOrderCommandHandler : BaseHandler, IRequestHandler<PatchOrderCommand, ApiResponse<Unit>>
    {
        public PatchOrderCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<Unit>> Handle(PatchOrderCommand request, CancellationToken cancellationToken)
        {

            var order = await _applicationDbContext.Orders
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            order.Status = request.Status;




            if (request.Note != null)
            {
                order.Notes.Add(request.Note);
            }

            order.UpdatedBy = _currentUserService.UserId;
            order.UpdatedAt = DateTime.Now;
            _applicationDbContext.Orders
                .Update(order);
            await _applicationDbContext
                .SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }

}