using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Customers.Queries;

namespace QDryClean.Application.UseCases.Customers.Validators
{
    public class GetCustomerQueryValidator : AbstractValidator<GetByIdCustomerQuery>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetCustomerQueryValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Customer ID is required.")
                .GreaterThan(0).WithMessage("Customer ID must be greater than 0")
                .MustAsync(async (query, id, cancellationToken) =>
                    await _dbContext.Customers.AnyAsync(c => c.Id == id && c.DeletedAt == null && c.DeletedBy == null, cancellationToken)
                ).WithMessage("Customer with this ID does not exist");
        }
    }
}
