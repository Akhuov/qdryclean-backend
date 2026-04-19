using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Items.Commands;

namespace QDryClean.Application.UseCases.Items.Validations
{
    public class PatchItemStatusCommandValidator
        : AbstractValidator<PatchItemStatusCommand>
    {
        public PatchItemStatusCommandValidator(IApplicationDbContext dbContext)
        {   

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Item status is required.")
                .NotEmpty().WithMessage("Item status cannot be empty.");

        }
    }
}
