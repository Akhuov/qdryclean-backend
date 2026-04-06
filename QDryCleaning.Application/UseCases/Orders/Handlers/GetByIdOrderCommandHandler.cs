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
    public class GetByIdOrderCommandHandler : CommandHandlerBase, IRequestHandler<GetByIdOrderQuery, ApiResponse<OrderDetailsDto>>
    {
        public GetByIdOrderCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<OrderDetailsDto>> Handle(GetByIdOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders
                .AsNoTracking()
                .Where(o => o.Id == request.Id)
                .WhereNotDeleted()
                .Select(o => new OrderDetailsDto
                {
                    Id = o.Id,
                    Customer = new CustomerDto
                    {
                        Id = o.Customer.Id,
                        FullName = o.Customer.FullName,
                        PhoneNumber = o.Customer.PhoneNumber,
                        AdditionalPhoneNumber = o.Customer.AdditionalPhoneNumber
                    },
                    ReceiptNumber = o.ReceiptNumber,
                    Status = o.Status,
                    ExpectedCompletionDate = o.ExpectedCompletionDate,
                    CreatedAt = DateOnly.FromDateTime(o.CreatedAt),
                    Notes = o.Notes,
                    Items = o.Items.Select(i => new ItemViewModel
                    {
                        Id = i.Id,
                        ItemType = new ItemTypeDto
                        {
                            Id = i.ItemType.Id,
                            Name = i.ItemType.Name,
                            Cost = i.ItemType.Charge.Cost
                        },
                        Colour = i.Colour,
                        BrandName = i.BrandName,
                        Description = i.Description,
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return ApiResponseFactory.Ok(_mapper.Map<OrderDetailsDto>(order));
        }
    }
}
