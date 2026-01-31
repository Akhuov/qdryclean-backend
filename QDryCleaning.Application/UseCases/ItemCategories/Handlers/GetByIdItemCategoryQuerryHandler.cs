using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.ItemCategories.Querries;

namespace QDryClean.Application.UseCases.ItemCategories.Handlers
{
    public class GetByIdItemCategoryQuerryHandler : CommandHandlerBase, IRequestHandler<GetByIdItemCategoryQuerry, ApiResponse<ItemCategoryDto>>
    {
        public GetByIdItemCategoryQuerryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ItemCategoryDto>> Handle(GetByIdItemCategoryQuerry request, CancellationToken cancellationToken)
        {
            var itemCategory = await _applicationDbContext.ItemCategories.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return ApiResponseFactory.Ok(new ItemCategoryDto() { Id = itemCategory.Id, Name = itemCategory.Name, Description = itemCategory.Description });
        }
    }
}
