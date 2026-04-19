using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.ItemCategories.Commands;

namespace QDryClean.Application.UseCases.ItemCategories.Handlers
{
    public class UpdateItemCategoryCommandHandler : BaseHandler, IRequestHandler<UpdateItemCategoryCommand, ApiResponse<ItemCategoryDto>>
    {
        public UpdateItemCategoryCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ItemCategoryDto>> Handle(UpdateItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemCategory = await _applicationDbContext.ItemCategories.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            itemCategory.Name = request.Name;
            itemCategory.Description = request.Description;
            itemCategory.UpdatedBy = _currentUserService.UserId;
            itemCategory.UpdatedAt = DateTime.Now;

            _applicationDbContext.ItemCategories.Update(itemCategory);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(new ItemCategoryDto() { Id = itemCategory.Id, Name = itemCategory.Name, Description = itemCategory.Description });
        }
    }
}
