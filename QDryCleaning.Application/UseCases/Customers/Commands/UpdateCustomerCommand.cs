using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<ApiResponse<CustomerDto>> 
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? AdditionalPhoneNumber { get; set; }
        public decimal Points { get; set; } = decimal.Zero;
    }
}
