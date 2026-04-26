using FluentValidation;
using QDryClean.Application.UseCases.Payments.Commands;

namespace QDryClean.Application.UseCases.Payments.Validations
{
    public class CreatePaymentByOrderIdCommandValidator :
        AbstractValidator<CreatePaymentByOrderIdCommand>
    {

        public CreatePaymentByOrderIdCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                    .WithMessage("Идентификатор заказа должен быть больше 0.");
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                    .WithMessage("Сумма оплаты должна быть больше 0.");
            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                    .WithMessage("Метод оплаты не может быть пустым.");
        }
    }
}
