using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Payments.Commands;
using QDryClean.Domain.Entities;
using QDryClean.Domain.Enums;

namespace QDryClean.Application.UseCases.Payments.Handlers
{
    public class CreatePaymentByOrderIdCommandHandler : BaseHandler, IRequestHandler<CreatePaymentByOrderIdCommand, ApiResponse<Unit>>
    {

        public CreatePaymentByOrderIdCommandHandler(
            IApplicationDbContext applicationDbContext,
            IReceiptGenerator receiptGenerator,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<Unit>> Handle(CreatePaymentByOrderIdCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _applicationDbContext.OrderInvoices
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.OrderId == request.OrderId, cancellationToken);

            if (invoice == null)
                throw new NotFoundException("Счет не найден.");

            if (invoice.PaymentStatus == PaymentStatus.Paid)
                throw new InvalidOperationException("Счет уже полностью оплачен.");

            var totalPaid = invoice.Payments?.Sum(x => x.Amount) ?? 0;

            var remaining = invoice.TotalCost - totalPaid;

            if (request.Amount > remaining)
                throw new InvalidOperationException($"Сумма оплаты превышает оставшуюся сумму счета {remaining}.");

            var payment = new Payment
            {
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                InvoiceId = invoice.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUserService.UserId
            };

            invoice.Payments.Add(payment);

            totalPaid += request.Amount;

            invoice.PaymentStatus = totalPaid == invoice.TotalCost
                ? PaymentStatus.Paid
                : PaymentStatus.Partial;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponseFactory.Ok(Unit.Value);
        }
    }
}
