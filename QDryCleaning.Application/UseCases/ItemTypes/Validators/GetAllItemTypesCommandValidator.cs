using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.ItemTypes.Quarries;

namespace QDryClean.Application.UseCases.ItemTypes.Validators
{
    public class GetAllItemTypesCommandValidator
        : AbstractValidator<GetAllItemTypesCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetAllItemTypesCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Request cannot be null")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.ItemTypes.AnyAsync(
                        c => c.DeletedAt == null 
                            && c.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Item Types not found!");
        }
    }
}
