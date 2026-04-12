using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.Dtos.Orders;
using QDryClean.Application.UseCases.Orders.Queries;
using QDryClean.Application.ViewModels;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetByIdOrderQueryHandler : CommandHandlerBase, IRequestHandler<GetByIdOrderQuery, ApiResponse<OrderDetailsDto>>
    {
        private readonly IReceiptGenerator _receiptGenerator;

        public GetByIdOrderQueryHandler(
            IApplicationDbContext applicationDbContext,
            IReceiptGenerator receiptGenerator,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper)
        {
            _receiptGenerator = receiptGenerator;
        }

        public async Task<ApiResponse<OrderDetailsDto>> Handle(GetByIdOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Invoice)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ItemType)
                        .ThenInclude(it => it.Charge)
                .Where(o => o.Id == request.Id)
                .WhereNotDeleted()
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
                return ApiResponseFactory.Fail<OrderDetailsDto>(1001, "Order not found");

            var receiptBase64 = _receiptGenerator.GenerateEscPos(order);

            var orderDto = new OrderDetailsDto
            {
                Id = order.Id,
                Invoice = new InvoiceDto
                {
                    Id = order.Invoice?.Id ?? 0,
                    Discount = order.Invoice?.Discount ?? 0,
                    Notes = order.Invoice?.Notes,
                    PaymentStatus = order.Invoice?.PaymentStatus ?? 0,
                    TotalCost = order.Invoice?.TotalCost ?? 0
                },
                Customer = new CustomerDto
                {
                    Id = order.Customer?.Id ?? 0,
                    FullName = order.Customer?.FullName,
                    PhoneNumber = order.Customer?.PhoneNumber,
                    AdditionalPhoneNumber = order.Customer?.AdditionalPhoneNumber
                },
                ReceiptNumber = order.ReceiptNumber,
                ReceiptBase64 = receiptBase64,
                Status = order.Status,
                ExpectedCompletionDate = order.ExpectedCompletionDate,
                CreatedAt = DateOnly.FromDateTime(order.CreatedAt),
                Notes = order.Notes ?? new List<string>(),
                Items = order.Items?.Select(i => new ItemViewModel
                {
                    Id = i.Id,
                    ItemType = new ItemTypeDto
                    {
                        Id = i.ItemType?.Id ?? 0,
                        Name = i.ItemType?.Name,
                        Cost = i.ItemType?.Charge?.Cost ?? 0
                    },
                    Colour = i.Colour,
                    BrandName = i.BrandName,
                    Description = i.Description,
                    Status = i.Status
                }).ToList() ?? new List<ItemViewModel>()
            };

            return ApiResponseFactory.Ok(orderDto);
        }
    }
}