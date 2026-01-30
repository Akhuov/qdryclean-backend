using FluentValidation;
using QDryClean.Application.UseCases.ItemTypes.Commands;

namespace QDryClean.Application.UseCases.ItemTypes.Validators
{
    public class UpdateItemTypeCommandValidator
        : AbstractValidator<UpdateItemTypeCommand>
    {
        public UpdateItemTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Item Type ID is required.")
                .NotEmpty().WithMessage("Item Type ID is required.")
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Item Type Name is required.")
                .NotEmpty().WithMessage("Item Type Name is required.")
                .MaximumLength(100).WithMessage("Item Type Name must not exceed 100 characters.");
        }
    }
}
