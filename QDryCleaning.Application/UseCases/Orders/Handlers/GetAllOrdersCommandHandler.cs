using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Orders.Queries;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetAllOrdersCommandHandler : CommandHandlerBase, IRequestHandler<GetAllOrdersQuery, ApiResponse<List<OrderDto>>>
    {
        public GetAllOrdersCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _applicationDbContext.Orders.AsNoTracking().Include(x => x.Items).ToListAsync();

            var list_of_orderDtos = new List<OrderDto>();
            foreach (var invoice in orders)
            {
                list_of_orderDtos.Add(_mapper.Map<OrderDto>(invoice));
            }

            return ApiResponseFactory.Ok(list_of_orderDtos);
        }
    }
}