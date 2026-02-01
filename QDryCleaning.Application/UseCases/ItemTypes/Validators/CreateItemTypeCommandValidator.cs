using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.ItemTypes.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.ItemTypes.Validators
{
    public class CreateItemTypeCommandValidator
        : AbstractValidator<CreateItemTypeCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public CreateItemTypeCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Item Type Name is required.")
                .MaximumLength(100)
                    .WithMessage("Item Type Name must not exceed 100 characters.");
                RuleFor(x => x.Name)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Item Type Name is required.")
                    .MaximumLength(100).WithMessage("Item Type Name must not exceed 100 characters.")
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


            RuleFor(x => x.ItemCategoryId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Item Category ID is required.")
                .NotNull()
                    .WithMessage("Item Category ID is required.")
                .GreaterThan(0)
                    .WithMessage("Item Category ID must be greater than zero.")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.ItemCategories.AnyAsync(
                        c => c.Id == id 
                            && c.DeletedAt == null 
                            && c.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Item Category with this ID does not exist");


            RuleFor(x => x.ChargeId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                    .WithMessage("Charge ID must be greater than zero.")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.Charges.AnyAsync(
                        c => c.Id == id
                            && c.DeletedAt == null
                            && c.DeletedBy == null, cancellationToken);
                })
                    .When(x => x.ChargeId.HasValue)
                    .WithMessage("Charge with this ID does not exist");
        }
    }
}
