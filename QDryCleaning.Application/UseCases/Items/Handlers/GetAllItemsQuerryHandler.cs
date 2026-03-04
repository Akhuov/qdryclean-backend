using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Items.Querries;

namespace QDryClean.Application.UseCases.Items.Handlers
{
    public class GetAllItemsQuerryHandler : CommandHandlerBase, IRequestHandler<GetAllItemsQuerry, ApiResponse<List<ItemDto>>>
    {
        public GetAllItemsQuerryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<List<ItemDto>>> Handle(GetAllItemsQuerry request, CancellationToken cancellationToken)
        {

            var items = await _applicationDbContext.Items
                .Where(c => c.DeletedAt == null && c.DeletedBy == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var listOfItemDtos = new List<ItemDto>();
            foreach (var item in items)
            {
                listOfItemDtos.Add(_mapper.Map<ItemDto>(item));
            }

            return ApiResponseFactory.Ok(listOfItemDtos);
        }
    }
}
