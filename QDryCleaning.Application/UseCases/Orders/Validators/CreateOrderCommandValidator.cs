using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Orders.Validators
{
    public class CreateOrderCommandValidator
        : AbstractValidator<CreateOrderCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateOrderCommandValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("ID клиента является обязательным.")
                .GreaterThan(0)
                .MustAsync(async (command, customerId, CancellationToken) =>
                {
                    return await _dbContext.Customers.AnyAsync(c => c.Id == customerId && c.DeletedAt == null && c.DeletedBy == null, CancellationToken);
                }).WithMessage("Клиент с таким ID не существует");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Форма заказа должна содержать хотя бы один предмет.")
                .Must(items => items.All(i => i.ItemTypeId > 0)).WithMessage("Все предметы должны иметь действительный ItemTypeId.");

            RuleFor(x => x)
                .Must((command, CancellationToken) =>
                {
                    if (command.PaymentStatus != PaymentStatus.NotPaid 
                        && command.PaymentStatus != PaymentStatus.Paid 
                        && command.Payment == null)
                    {
                        return false;
                    }
                    return true;
                }).WithMessage("Детали оплаты должны быть предоставлены, если статус оплаты не 'Не оплачено'");
        }
    }
}