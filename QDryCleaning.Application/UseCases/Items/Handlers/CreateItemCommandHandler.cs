using AutoMapper;
using MediatR;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Items.Commands;
using QDryClean.Domain.Entities;

namespace QDryClean.Application.UseCases.Items.Handlers
{
    public class CreateItemCommandHandler : BaseHandler, IRequestHandler<CreateItemCommand, ApiResponse<ItemDto>>
    {
        public CreateItemCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }
        public async Task<ApiResponse<ItemDto>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Item>(request);
            item.CreatedBy = _currentUserService.UserId;
            item.CreatedAt = DateTime.UtcNow;
            await _applicationDbContext.Items.AddAsync(item, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ItemDto>(item));
        }
    }
}
