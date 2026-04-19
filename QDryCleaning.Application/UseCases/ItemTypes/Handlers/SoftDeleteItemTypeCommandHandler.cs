using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.ItemTypes.Commands;

namespace QDryClean.Application.UseCases.ItemTypes.Handlers
{
    public class SoftDeleteItemTypeCommandHandler : BaseHandler, IRequestHandler<SoftDeleteItemTypeCommand, ApiResponse<Unit>>
    {
        public SoftDeleteItemTypeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(SoftDeleteItemTypeCommand request, CancellationToken cancellationToken)
        {

            var itemType = await _applicationDbContext.ItemTypes.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            itemType.DeletedAt = DateTime.UtcNow;
            itemType.DeletedBy = _currentUserService.UserId;

            _applicationDbContext.ItemTypes.Update(itemType);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
