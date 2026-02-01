using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Items.Commands;

namespace QDryClean.Application.UseCases.Items.Validations
{
    public class CreateItemCommandValidator
        : AbstractValidator<CreateItemCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        public CreateItemCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.ItemTypeId)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Item type ID is required.")
                .NotEmpty()
                    .WithMessage("Item type ID is required.")
                .GreaterThan(0)
                    .WithMessage("Item category ID must be greater than zero.")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.ItemTypes.AnyAsync(c => c.Id == id 
                        && c.DeletedAt == null 
                        && c.DeletedBy == null, cancellationToken);
                }).WithMessage("Item Type with this Id not found!");

            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID must be greater than zero.")
                .MustAsync(async (command, id, cancellationToken) =>
                {
                    return await _dbContext.Orders.AnyAsync(c => c.Id == id
                        && c.DeletedAt == null
                        && c.DeletedBy == null, cancellationToken);
                })
                    .When(x => x.OrderId != null)
                    .WithMessage("Order with this ID does not exist");

            RuleFor(x => x.Colour)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Item name is required.")
                .NotEmpty()
                    .WithMessage("Item name is required.")
                .MaximumLength(100).WithMessage("Item name must not exceed 100 characters.");

            RuleFor(x => x.BrandName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Brand name is required.")
                .NotEmpty()
                    .WithMessage("Brand name is required.")
                .MaximumLength(100)
                    .WithMessage("Brand name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Description is required.")
                .NotEmpty()
                    .WithMessage("Description is required.")
                .MaximumLength(500)
                    .WithMessage("Description must not exceed 500 characters.");
        }
    }
}
