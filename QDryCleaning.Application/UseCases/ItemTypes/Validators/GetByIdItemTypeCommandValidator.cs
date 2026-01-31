using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.ItemTypes.Quarries;

namespace QDryClean.Application.UseCases.ItemTypes.Validators
{
    public class GetByIdItemTypeCommandValidator
        : AbstractValidator<GetByIdItemTypeCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetByIdItemTypeCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Item Type ID is required.")
                .GreaterThan(0)
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.ItemTypes.AnyAsync(
                        i => i.Id == id 
                            && i.DeletedAt == null 
                            && i.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Item Type with this ID does not exist");
        }
    }
}
