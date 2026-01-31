using AutoMapper;
using MediatR;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.ItemCategories.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.ItemCategories.Handlers
{
    public class CreateItemTypeCommandHandler : CommandHandlerBase, IRequestHandler<CreateItemCategoryCommand, ApiResponse<ItemCategoryDto>>
    {
        public CreateItemTypeCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ItemCategoryDto>> Handle(CreateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemCategory = new ItemCategory()
            {
                Name = request.Name,
                Description = request.Description,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.Now
            };
            await _applicationDbContext.ItemCategories.AddAsync(itemCategory, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(new ItemCategoryDto() { Id = itemCategory.Id, Name = itemCategory.Name, Description = itemCategory.Description });
        }
    }
}