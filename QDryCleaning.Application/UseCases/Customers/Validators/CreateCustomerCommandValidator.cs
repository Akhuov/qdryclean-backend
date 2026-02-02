using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Helpers;
using QDryClean.Application.UseCases.Customers.Commands;

namespace QDryClean.Application.UseCases.Customers.Validators;

public class CreateCustomerCommandValidator
    : AbstractValidator<CreateCustomerCommand>
{
    private readonly IApplicationDbContext _dbContext;
    public CreateCustomerCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotNull()
                .WithMessage("First Name is required")
            .NotEmpty()
                .WithMessage("First Name is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone Number is required")
            .Matches(@"^(?:\+998|998)?\s?\d{2}\s?\d{3}\s?\d{2}\s?\d{2}$")
            .WithMessage("Invalid phone number format")
            .MustAsync(async (command, phone, cancellationToken) =>
            {
                phone = PhoneNumberHelper.NormalizePhoneNumber(phone);
                return !await _dbContext.Customers
                    .AnyAsync(c => c.PhoneNumber == phone, cancellationToken);
            })
            .WithMessage("Customer with this phone number already exists");

        RuleFor(x => x.AdditionalPhoneNumber)
            .Matches(@"^(?:\+998|998)?\s?\d{2}\s?\d{3}\s?\d{2}\s?\d{2}$")
            .WithMessage("Invalid phone number format");
    }
}