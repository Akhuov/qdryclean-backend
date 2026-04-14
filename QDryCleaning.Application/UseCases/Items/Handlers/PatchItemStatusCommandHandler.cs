using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Items.Commands;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Items.Handlers
{
    public class PatchItemStatusCommandHandler : CommandHandlerBase, IRequestHandler<PatchItemStatusCommand, ApiResponse<Unit>>
    {
        public PatchItemStatusCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(PatchItemStatusCommand request, CancellationToken cancellationToken)
        {
            var item = await _applicationDbContext.Items
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (item == null)
            {
                throw new NotFoundException("Item not found");
            }

            item.Status = request.Status;

            item.UpdatedBy = _currentUserService.UserId;
            item.UpdatedAt = DateTime.UtcNow;

            _applicationDbContext.Items.Update(item);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);


            var order = await _applicationDbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);

            foreach (var i in order.Items)
            {
                if (i.Status != ItemStatus.Packed)
                {
                    return ApiResponseFactory.Ok(Unit.Value);
                }
            }

            order.Status = OrderStatus.Ready;
            order.UpdatedBy = _currentUserService.UserId;
            order.UpdatedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);

        }
    }
}
