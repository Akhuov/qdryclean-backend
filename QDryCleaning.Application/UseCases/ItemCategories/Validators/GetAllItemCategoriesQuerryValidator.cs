using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.ItemCategories.Querries;

namespace QDryClean.Application.UseCases.ItemCategories.Validators
{
    public class GetAllItemCategoriesQuerryValidator
        : AbstractValidator<GetAllItemCategoriesQuerry>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetAllItemCategoriesQuerryValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x)
                .NotNull().WithMessage("Request cannot be null")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.ItemCategories.AnyAsync(c => c.DeletedAt == null && c.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Item Categories not found!"); ;
        }
    }
}
