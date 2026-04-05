using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Orders.Commands;

namespace QDryClean.Application.UseCases.Orders.Validators
{
    public class UpdateOrderCommandValidator
        : AbstractValidator<PatchOrderCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateOrderCommandValidator(IApplicationDbContext dbContext)
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
                .WithMessage("Order with this ID does not exist");

            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Notes is required.");
        }
    }
}
