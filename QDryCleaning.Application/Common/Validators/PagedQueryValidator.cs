using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;

namespace QDryClean.Application.Common.Validators
{
    public class PagedQueryValidator<TQuery, TEntity>
        : AbstractValidator<TQuery>
        where TQuery : IPagedQuery
        where TEntity : class
    {
        public PagedQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");

            RuleFor(x => x)
                .NotNull()
                    .WithMessage("Request cannot be null")
                .MustAsync(async (query, cancellationToken) =>
                {
                    var totalCount = await dbContext.Set<TEntity>()
                        .CountAsync(cancellationToken);

                    var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

                    return totalPages == 0 || query.Page <= totalPages;
                })
                .WithMessage("Requested page number exceeds available pages.");
        }
    }
}
