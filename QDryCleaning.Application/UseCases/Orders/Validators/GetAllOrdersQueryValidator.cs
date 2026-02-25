using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Validators;
using QDryClean.Application.UseCases.Orders.Queries;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Orders.Validators
{
    public class GetAllOrdersQueryValidator
        : PagedQueryValidator<GetAllOrdersQuery, Order>
    {
        public GetAllOrdersQueryValidator(IApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
