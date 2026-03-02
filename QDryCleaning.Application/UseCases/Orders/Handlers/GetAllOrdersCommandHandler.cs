using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Pagination;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Orders.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QDryClean.Application.UseCases.Orders.Handlers
{
    public class GetAllOrdersCommandHandler : CommandHandlerBase, IRequestHandler<GetAllOrdersQuery, ApiResponse<PagedResult<OrderDto>>>
    {
        public GetAllOrdersCommandHandler(
           IApplicationDbContext applicationDbContext,
           ICurrentUserService currentUserService,
           IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<PagedResult<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var baseQuery = _applicationDbContext.Orders
                .Where(x => x.DeletedAt == null && x.DeletedBy == null)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim();
                var sLower = s.ToLower();

                // если это число — расширяем поиск по числовым полям
                var isNumber = int.TryParse(s, out var n);

                baseQuery = baseQuery.Where(o =>
                    (isNumber && (o.Id == n || o.ReceiptNumber == n || o.CustomerId == n)) ||
                    // текстовый поиск (пример)
                    o.Notes.Any(note => note.ToLower().Contains(sLower))
                // если есть связанный Customer:
                // || o.Customer.FullName.ToLower().Contains(sLower)
                );
            }

            var pagedResult = await baseQuery
                .AsNoTracking()
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Id)
                .ToPagedResultAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken
                );

            return ApiResponseFactory.Ok(pagedResult);
        }
    }
}