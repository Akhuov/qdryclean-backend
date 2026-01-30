using FluentValidation;
using QDryClean.Application.UseCases.Charges.Commands;

namespace QDryClean.Application.UseCases.Charges.Validators
{
    public class SoftDeleteChargeCommandValidator
        : AbstractValidator<SoftDeleteChargeCommand>
    {
        public SoftDeleteChargeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Charge ID is required.")
                .GreaterThan(0).WithMessage("Charge ID must be greater than zero.");
        }
    }
}
