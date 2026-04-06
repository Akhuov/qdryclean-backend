using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class SoftDeleteOrderCommandHandler : CommandHandlerBase, IRequestHandler<SoftDeleteOrderCommand, ApiResponse<Unit>>
    {
        public SoftDeleteOrderCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<Unit>> Handle(SoftDeleteOrderCommand request, CancellationToken cancellationToken)
        {

            var order = await _applicationDbContext.Orders
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            order.Status = OrderStatus.Canceled;
            order.DeletedAt = DateTime.UtcNow;
            order.DeletedBy = _currentUserService.UserId;

            _applicationDbContext.Orders.Update(order);
            await _applicationDbContext
                .SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
