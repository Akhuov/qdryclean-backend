using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.Items.Commands
{
    public class UpdateItemCommand : IRequest<ApiResponse<ItemDto>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Colour { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
    }
}
