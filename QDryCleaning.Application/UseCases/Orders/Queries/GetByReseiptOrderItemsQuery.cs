using MediatR;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Orders.Queries
{
    public class GetByReseiptOrderItemsQuery : IRequest<ApiResponse<List<ItemDto>>>
    {
        public int ReseiptNumber { get; set; }
    }
}
