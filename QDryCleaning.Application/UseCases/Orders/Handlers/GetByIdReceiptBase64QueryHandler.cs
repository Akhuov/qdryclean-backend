using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.UseCases.Orders.Queries;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetByIdReceiptBase64QueryHandler : BaseHandler, IRequestHandler<GetByIdReceiptBase64Query, ApiResponse<string>>
    {
        private readonly IReceiptGenerator _receiptGenerator;

        public GetByIdReceiptBase64QueryHandler(
            IApplicationDbContext applicationDbContext,
            IReceiptGenerator receiptGenerator,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper)
        {
            _receiptGenerator = receiptGenerator;
        }
        public async Task<ApiResponse<string>> Handle(GetByIdReceiptBase64Query request, CancellationToken cancellationToken)
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
                return ApiResponseFactory.Fail<string>(1001, "Order not found");

            var receiptBase64 = _receiptGenerator.GenerateEscPos(order);
            return ApiResponseFactory.Ok(receiptBase64);
        }
    }
}
