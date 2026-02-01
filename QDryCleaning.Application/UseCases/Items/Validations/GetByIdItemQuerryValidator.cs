using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Items.Querries;

namespace QDryClean.Application.UseCases.Items.Validations
{
    public class GetByIdItemQuerryValidator
        : AbstractValidator<GetByIdItemQuerry>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetByIdItemQuerryValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Item Id is required.")
                .GreaterThan(0)
                    .WithMessage("Item Id must be greater than zero.")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.Items.AnyAsync(c => c.Id == id
                        && c.DeletedAt == null
                        && c.DeletedBy == null, cancellationToken);
                })
                    .WithMessage("Item with Id not found!");
        }
    }
}
