using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Domain.Enums;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<ApiResponse<OrderDto>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public ProcessStatus ProcessStatus { get; set; }
        public string? Note { get; set; }
    }
}
