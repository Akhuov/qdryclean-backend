using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Items.Commands;

namespace QDryClean.Application.UseCases.Items.Handlers
{
    public class PatchItemCommandHandler : CommandHandlerBase, IRequestHandler<PatchItemCommand, ApiResponse<ItemDto>>
    {
        public PatchItemCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ItemDto>> Handle(PatchItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _applicationDbContext.Items.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return ApiResponseFactory.Fail<ItemDto>(1001, "Item not found");
            }

            item.Status = request.Status;

            item.UpdatedBy = _currentUserService.UserId;
            item.UpdatedAt = DateTime.UtcNow;

            _applicationDbContext.Items.Update(item);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ItemDto>(item));
        }
    }
}
