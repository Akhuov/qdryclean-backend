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
    public class GetAllItemTypesCommandHandler : CommandHandlerBase, IRequestHandler<GetAllItemTypesCommand, ApiResponse<List<ItemTypeDto>>>
    {
        public GetAllItemTypesCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<List<ItemTypeDto>>> Handle(GetAllItemTypesCommand request, CancellationToken cancellationToken)
        {

            var itemTypes = await _applicationDbContext.ItemTypes.Include(x=> x.Charge).ToListAsync();

            var listOfItemTypeDtos = new List<ItemTypeDto>();
            foreach (var itemType in itemTypes)
            {
                listOfItemTypeDtos.Add( new ItemTypeDto()
                {
                    Id = itemType.Id,
                    Name = itemType.Name,
                    Cost = itemType.Charge.Cost
                }
                    );
            }

            return ApiResponseFactory.Ok(listOfItemTypeDtos);
        }
    }
}
