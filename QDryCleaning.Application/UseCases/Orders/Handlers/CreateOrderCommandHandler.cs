using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos.Orders;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class CreateOrderCommandHandler : CommandHandlerBase, IRequestHandler<CreateOrderCommand, ApiResponse<OrderDto>>
    {
        private readonly IInvoiceFactory _invoiceFactory;
        public CreateOrderCommandHandler(
           IApplicationDbContext applicationDbContext,
           IInvoiceFactory invoiceFactory,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper)
        {
            _invoiceFactory = invoiceFactory;
        }
        public async Task<ApiResponse<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var itemTypeIds = request.Items
               .Select(x => x.ItemTypeId)
               .Distinct()
               .ToList();

            var itemTypes = await _applicationDbContext.ItemTypes
                .Where(x => itemTypeIds.Contains(x.Id))
                .WhereNotDeleted()
                .Include(x => x.Charge)
                .ToListAsync(cancellationToken);

            var order = _mapper.Map<Order>(request);

            order.ExpectedCompletionDate = request.ExpectedCompletionDate ??
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3));
            order.CreatedBy = _currentUserService.UserId;
            order.CreatedAt = DateTime.Now;

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

            var invoice = _invoiceFactory.Create(order, itemTypes);

            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
            await _applicationDbContext.OrderInvoices.AddAsync(invoice, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponseFactory.Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
