using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.ItemTypes.Quarries;

namespace QDryClean.Application.UseCases.ItemTypes.Handlers
{
    public class GetByIdItTypeCommandHandler : CommandHandlerBase, IRequestHandler<GetByIdItemTypeCommand, ApiResponse<ItemTypeDto>>
    {
        public GetByIdItTypeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<ItemTypeDto>> Handle(GetByIdItemTypeCommand request, CancellationToken cancellationToken)
        {
            var itemType = await _applicationDbContext.ItemTypes.Include(x=>x.Charge).FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return ApiResponseFactory.Ok(new ItemTypeDto() 
            { 
                Id = itemType.Id,
                Name = itemType.Name,
                Cost = itemType.Charge.Cost
            });
        }
    }
}
