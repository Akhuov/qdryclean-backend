using MediatR;
using QDryClean.Application.Common.Responses;

namespace QDryClean.Application.UseCases.Orders.Queries
{
    public class GetByIdReceiptBase64Query : IRequest<ApiResponse<string>>
    {
        public int Id { get; set; }
    }
}
