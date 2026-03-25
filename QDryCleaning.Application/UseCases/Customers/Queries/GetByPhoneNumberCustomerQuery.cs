using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Customers.Queries
{
    public class GetByPhoneNumberCustomerQuery : IRequest<ApiResponse<CustomerDto>>
    {
        public string PhoneNumber { get; set; }
    }
}
