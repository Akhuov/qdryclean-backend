using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Users.Quarries;

namespace QDryClean.Application.UseCases.Users.Handlers
{
    public class GetByIdUserCommandHandler : BaseHandler, IRequestHandler<GetByIdUserCommand, UserDto>
    {
        public GetByIdUserCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<UserDto> Handle(GetByIdUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user is not null)
                return _mapper.Map<UserDto>(user);
            return null;
        }
    }
}
