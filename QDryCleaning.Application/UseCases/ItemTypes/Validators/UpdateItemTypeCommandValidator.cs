using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.ItemTypes.Commands;

namespace QDryClean.Application.UseCases.ItemTypes.Validators
{
    public class UpdateItemTypeCommandValidator
        : AbstractValidator<UpdateItemTypeCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public UpdateItemTypeCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Item Type ID is required.")
                .NotEmpty()
                    .WithMessage("Item Type ID is required.")
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Item Type Name is required.")
                .NotEmpty()
                    .WithMessage("Item Type Name is required.")
                .MaximumLength(100)
                    .WithMessage("Item Type Name must not exceed 100 characters.")
                .MustAsync(async (command, name, cancellationToken) =>
                {
                    var normalizedName = name.Trim();

                    return !await _dbContext.ItemTypes.AnyAsync(
                        c => c.Name.ToLower() == normalizedName.ToLower()
                          && c.DeletedAt == null
                          && c.DeletedBy == null,
                        cancellationToken);
                })
                .WithMessage("Item Type with this name already exists");
        }
    }
}
