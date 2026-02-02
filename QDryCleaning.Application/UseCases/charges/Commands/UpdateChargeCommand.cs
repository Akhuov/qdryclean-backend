using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Charges.Commands
{
    public class UpdateChargeCommand : IRequest<ApiResponse<ChargeDto>> 
    {
        [JsonIgnore]
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string? Name { get; set; }
    }
}
