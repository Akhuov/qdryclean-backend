using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.ItemCategories.Commands
{
    public class CreateItemCategoryCommand : IRequest<ApiResponse<ItemCategoryDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
