using FluentValidation;
using QDryClean.Application.UseCases.Dashboard.Queries;
using System.Globalization;

namespace QDryClean.Application.UseCases.Dashboard.Validations
{
    public class OrdersSummaryQueryValidations
        : AbstractValidator<OrdersSummaryQuery>
    {
        public OrdersSummaryQueryValidations()
        {
            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("From is required")
                .Must(BeValidDateFormat)
                .WithMessage("Invalid From format. Expected yyyy-MM-dd");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("To is required")
                .Must(BeValidDateFormat)
                .WithMessage("Invalid To format. Expected yyyy-MM-dd");

            RuleFor(x => x)
                .Must(BeValidRange)
                .WithMessage("From must be less than or equal to To");
        }

        private bool BeValidDateFormat(string date)
        {
            return DateTime.TryParseExact(
                date,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }

        private bool BeValidRange(OrdersSummaryQuery query)
        {
            if (!BeValidDateFormat(query.From) || !BeValidDateFormat(query.To))
                return true; // формат уже отловится выше

            var from = DateTime.ParseExact(query.From, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var to = DateTime.ParseExact(query.To, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return from <= to;
        }
    }
}
