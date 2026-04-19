using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Orders.Commands;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class DeleteOrderCommandHandler : BaseHandler, IRequestHandler<DeleteOrderCommand, ApiResponse<Unit>>
    {
        public DeleteOrderCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<Unit>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {

            var order = await _applicationDbContext.Orders.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            _applicationDbContext.Orders.Remove(order);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
