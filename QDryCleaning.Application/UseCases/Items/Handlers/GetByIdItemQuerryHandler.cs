using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Common.Responses;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Items.Querries;

namespace QDryClean.Application.UseCases.Items.Handlers
{
    internal class GetByIdItemQuerryHandler : CommandHandlerBase, IRequestHandler<GetByIdItemQuerry, ApiResponse<ItemDto>>
    {
        public GetByIdItemQuerryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<ApiResponse<ItemDto>> Handle(GetByIdItemQuerry request, CancellationToken cancellationToken)
        {
            var item = await _applicationDbContext.Items.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            return ApiResponseFactory.Ok(_mapper.Map<ItemDto>(item));
        }
    }
}
