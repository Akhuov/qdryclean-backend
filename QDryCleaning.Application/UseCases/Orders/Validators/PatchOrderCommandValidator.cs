using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Orders.Commands;

namespace QDryClean.Application.UseCases.Orders.Validators
{
    public class PatchOrderCommandValidator
        : AbstractValidator<PatchOrderCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public PatchOrderCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Order ID is required.")
                .GreaterThan(0)
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.Orders
                        .AnyAsync(o => o.Id == id && o.DeletedAt == null && o.DeletedBy == null, cancellationToken);
                })
                .WithMessage("Order with this ID does not exist");

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .IsInEnum().WithMessage("Invalid order status.")
                .NotNull().WithMessage("Order status is required.")
                .NotEmpty().WithMessage("Order status is required.");

            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Notes is required.");
        }
    }
}
