using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.Dtos.Orders;
using QDryClean.Application.UseCases.Orders.Commands;
using QDryClean.Domain.Entities;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class CreateOrderCommandHandler : BaseHandler, IRequestHandler<CreateOrderCommand, ApiResponse<OrderCreatedDto>>
    {
        private readonly IInvoiceFactory _invoiceFactory;
        private readonly IReceiptGenerator _receiptGenerator;

        public CreateOrderCommandHandler(
            IApplicationDbContext applicationDbContext,
            IReceiptGenerator receiptGenerator,
            IInvoiceFactory invoiceFactory,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper)
        {
            _invoiceFactory = invoiceFactory;
            _receiptGenerator = receiptGenerator;
        }

        public async Task<ApiResponse<OrderCreatedDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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

            if (itemTypes.Count != itemTypeIds.Count)
            {
                return ApiResponseFactory.Fail<OrderCreatedDto>(
                    1001,
                    "Один или несколько типов вещей не найдены.");
            }

            var customer = await _applicationDbContext.Customers
                .Where(x => x.Id == request.CustomerId)
                .WhereNotDeleted()
                .FirstOrDefaultAsync(cancellationToken);

            if (customer is null)
            {
                return ApiResponseFactory.Fail<OrderCreatedDto>(
                    1001,
                    "Клиент не найден.");
            }

            var order = _mapper.Map<Order>(request);

            order.Items ??= new List<Item>();
            order.Notes ??= new List<string>();

            order.Customer = customer;
            order.CustomerId = customer.Id;
            order.ExpectedCompletionDate = request.ExpectedCompletionDate ??
                                           DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3));
            order.Status = OrderStatus.Created;
            order.CreatedBy = _currentUserService.UserId;
            order.CreatedAt = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(request.Note))
            {
                order.Notes.Add(request.Note);
            }

            var items = _mapper.Map<List<Item>>(request.Items);

            foreach (var item in items)
            {
                var itemType = itemTypes.First(x => x.Id == item.ItemTypeId);

                item.Status = ItemStatus.Accepted;
                item.Order = order;
                item.ItemType = itemType;
            }

            order.Items = items;

            var receiptBase64 = _receiptGenerator.GenerateEscPos(order);

            var invoice = _invoiceFactory.Create(order, itemTypes, request.PaymentStatus);

            if (request.PaymentStatus != PaymentStatus.NotPaid)
            {
                var amount = invoice.TotalCost;

                if (request.PaymentStatus == PaymentStatus.Paid)
                {
                    amount = invoice.TotalCost;
                }

                var newPayment = new Payment
                {
                    Amount = amount,
                    PaymentMethod = request.Payment.PaymentMethod,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _currentUserService.UserId,
                    Invoice = invoice
                };

                if (invoice == null)
                    throw new Exception("Invoice is null");
                invoice.Payments.Add(newPayment);
            }

            await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
            await _applicationDbContext.OrderInvoices.AddAsync(invoice, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            var orderCreatedDto = new OrderCreatedDto
            {
                Id = order.Id,
                ReceiptNumber = order.ReceiptNumber,
                Customer = _mapper.Map<CustomerDto>(order.Customer),
                Invoice = _mapper.Map<InvoiceDto>(invoice),
                Status = order.Status,
                ExpectedCompletionDate = order.ExpectedCompletionDate,
                Items = _mapper.Map<List<ItemDto>>(order.Items)
            };

            return ApiResponseFactory.Ok(orderCreatedDto);
        }
    }
}