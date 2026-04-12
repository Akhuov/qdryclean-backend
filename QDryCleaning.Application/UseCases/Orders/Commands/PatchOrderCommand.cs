using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Domain.Enums;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Orders.Commands
{
    public class PatchOrderCommand : IRequest<ApiResponse<Unit>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public string Note { get; set; }
    }
}
