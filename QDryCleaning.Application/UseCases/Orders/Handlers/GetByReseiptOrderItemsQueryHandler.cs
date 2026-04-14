using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Orders.Queries;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetByReseiptOrderItemsQueryHandler : CommandHandlerBase, IRequestHandler<GetByReseiptOrderItemsQuery, ApiResponse<List<ItemDto>>>
    {
        public GetByReseiptOrderItemsQueryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<List<ItemDto>>> Handle(GetByReseiptOrderItemsQuery request, CancellationToken cancellationToken)
        {

            var order = await _applicationDbContext.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.ReceiptNumber == request.ReseiptNumber, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with receipt number {request.ReseiptNumber} not found.");
            }

            var items = _mapper.Map<List<ItemDto>>(order.Items);

            return ApiResponseFactory.Ok(items);
        }
    }
}
