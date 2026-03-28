using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Customers.Commands
{
    public class CreateCustomerCommand : IRequest<ApiResponse<CustomerDto>>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? AdditionalPhoneNumber { get; set; }
        public decimal Points { get; set; } = decimal.Zero;
    }
}
