using AutoMapper;
using MediatR;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class CreateOrderCommandHandler : CommandHandlerBase, IRequestHandler<CreateOrderCommand, ApiResponse<OrderDto>>
    {
        public CreateOrderCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);

            order.ExpectedCompletionDate =
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(request.DaysToCompletion ?? 3));
            order.CreatedBy = _currentUserService.UserId;
            order.CreatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(request.Note))
                order.Notes.Add(request.Note);

            // 1) замаппили items (ItemType/Order/OrderId игнорируются профилем)
            var items = _mapper.Map<List<Item>>(request.Items);

            // 2) связали items с order
            foreach (var item in items)
            {
                item.Order = order;
            }

            // 3) положили в заказ
            order.Items = items;

            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponseFactory.Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
