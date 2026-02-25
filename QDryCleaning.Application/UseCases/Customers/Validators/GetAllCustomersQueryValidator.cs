using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Validators;
using QDryClean.Application.UseCases.Customers.Queries;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Customers.Validators
{
    public class GetAllCustomersQueryValidator
    : PagedQueryValidator<GetAllCustomersQuery, Customer>
    {
        public GetAllCustomersQueryValidator(IApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
