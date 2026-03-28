using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Orders.Commands;

namespace QDryClean.Application.UseCases.Orders.Validators
{
    public class SoftDeleteOrderCommandValidator
        : AbstractValidator<SoftDeleteOrderCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public SoftDeleteOrderCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Order ID is required.")
                .GreaterThan(0)
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.Orders
                        .AnyAsync(o => o.Id == id && o.DeletedAt == null && o.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Order with this Id does not exist");
        }
    }
}
