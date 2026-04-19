using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.ItemCategories.Commands;

namespace QDryClean.Application.UseCases.ItemCategories.Handlers
{
    public class SoftDeleteItemCategoryCommandHandler : BaseHandler, IRequestHandler<SoftDeleteItemCategoryCommand, ApiResponse<Unit>>
    {
        public SoftDeleteItemCategoryCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<Unit>> Handle(SoftDeleteItemCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemCategory = await _applicationDbContext.ItemCategories.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            itemCategory.DeletedAt = DateTime.UtcNow;
            itemCategory.DeletedBy = _currentUserService.UserId;

            _applicationDbContext.ItemCategories.Update(itemCategory);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
