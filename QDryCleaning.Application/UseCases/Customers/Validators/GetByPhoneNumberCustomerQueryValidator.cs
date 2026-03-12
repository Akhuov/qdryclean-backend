using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Customers.Queries;

namespace QDryClean.Application.UseCases.Customers.Validators
{
    public class GetByPhoneNumberCustomerQueryValidator
        : AbstractValidator<GetByPhoneNumberCustomerQuery>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetByPhoneNumberCustomerQueryValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .MustAsync(async (phoneNumber, cancellationToken) =>
                {
                    var normalizedPhone = phoneNumber.Trim();

                    return await _dbContext.Customers.AnyAsync(
                        x => x.PhoneNumber == normalizedPhone &&
                             x.DeletedAt == null &&
                             x.DeletedBy == null,
                        cancellationToken);
                })
                .WithMessage("Customer with this phone number does not exist");
        }
    }
}
