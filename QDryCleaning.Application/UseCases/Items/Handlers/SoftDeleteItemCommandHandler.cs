using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Items.Commands;

namespace QDryClean.Application.UseCases.Items.Handlers
{
    public class SoftDeleteItemCommandHandler : BaseHandler, IRequestHandler<SoftDeleteItemCommand, ApiResponse<Unit>>
    {
        public SoftDeleteItemCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(SoftDeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _applicationDbContext.Items.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            item.DeletedAt = DateTime.UtcNow;
            item.DeletedBy = _currentUserService.UserId;

            _applicationDbContext.Items.Update(item);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}