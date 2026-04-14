using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Domain.Enums;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Items.Commands
{
    public class PatchItemStatusCommand : IRequest<ApiResponse<ItemDto>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public ItemStatus Status { get; set; }
    }
}
