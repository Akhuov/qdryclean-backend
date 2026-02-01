using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Items.Commands
{
    public class CreateItemCommand : IRequest<ApiResponse<ItemDto>>
    {
        public string? Colour { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
        public int ItemTypeId { get; set; }
        public int? OrderId { get; set; }
    }
}
