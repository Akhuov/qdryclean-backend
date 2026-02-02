using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using System.Text.Json.Serialization;

namespace QDryClean.Application.UseCases.ItemCategories.Commands
{
    public class UpdateItemCategoryCommand : IRequest<ApiResponse<ItemCategoryDto>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
