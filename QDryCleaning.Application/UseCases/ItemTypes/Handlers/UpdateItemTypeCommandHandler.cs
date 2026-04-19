using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.ItemTypes.Commands;

namespace QDryClean.Application.UseCases.ItemTypes.Handlers
{
    public class UpdateItemTypeCommandHandler : BaseHandler, IRequestHandler<UpdateItemTypeCommand, ApiResponse<UpdateItemTypeCommand>>
    {
        public UpdateItemTypeCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<UpdateItemTypeCommand>> Handle(UpdateItemTypeCommand request, CancellationToken cancellationToken)
        {

            var itemType = await _applicationDbContext.ItemTypes.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            itemType.Name = request.Name;

            itemType.UpdatedAt = DateTime.UtcNow;
            itemType.UpdatedBy = _currentUserService.UserId;

            _applicationDbContext.ItemTypes.Update(itemType);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponseFactory.Ok(new UpdateItemTypeCommand() { Id = itemType.Id, Name = itemType.Name });
        }
    }
}
