using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.ItemTypes.Commands
{
    public class CreateItemTypeCommand : IRequest<ApiResponse<ItemTypeDto>>
    {
        public string Name { get; set; }
        public int ItemCategoryId { get; set; }
        public int? ChargeId { get; set; } = null;
    }
}
