using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.ItemTypes.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.ItemTypes.Handlers
{
    public class CreateItemTypeCommandHandler : BaseHandler, IRequestHandler<CreateItemTypeCommand, ApiResponse<ItemTypeDto>>
    {
        public CreateItemTypeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<ItemTypeDto>> Handle(CreateItemTypeCommand request, CancellationToken cancellationToken)
        {
            var itemType = await _applicationDbContext.ItemTypes.FirstOrDefaultAsync(u => u.Name == request.Name, cancellationToken);

            itemType = _mapper.Map<ItemType>(request);
            itemType.CreatedBy = _currentUserService.UserId;
            itemType.CreatedAt = DateTime.Now;
            await _applicationDbContext.ItemTypes.AddAsync(itemType, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ItemTypeDto>(itemType));
        }
    }
}
