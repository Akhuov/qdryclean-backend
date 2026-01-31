using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Charges.Quarries;

namespace QDryClean.Application.UseCases.Charges.Validators
{
    public class GetByIdChargeCommandValidator
        : AbstractValidator<GetByIdChargeCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetByIdChargeCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Charge ID is required.")
                .NotEmpty().WithMessage("Charge ID is required.")
                .GreaterThan(0).WithMessage("Charge ID must be greater than 0.")
                .MustAsync(async (query, id, cancellationToken) =>
                    await _dbContext.Charges.AnyAsync(c => c.Id == id && c.DeletedAt == null && c.DeletedBy == null, cancellationToken)
                ).WithMessage("Charge with this ID does not exist");
        }
    }
}
