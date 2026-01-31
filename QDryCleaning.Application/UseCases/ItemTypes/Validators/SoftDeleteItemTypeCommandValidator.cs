using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.ItemTypes.Commands;

namespace QDryClean.Application.UseCases.ItemTypes.Validators
{
    public class SoftDeleteItemTypeCommandValidator
        : AbstractValidator<SoftDeleteItemTypeCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public SoftDeleteItemTypeCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Item Type ID is required")
                .NotEmpty()
                    .WithMessage("Item Type ID is required")
                .GreaterThan(0)
                    .WithMessage("Item Type Id must be greater than zero.")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.ItemTypes
                        .AnyAsync(it => it.Id == id 
                            && it.DeletedAt == null 
                            && it.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Item Type with this ID does not exist");
        }
    }
}
