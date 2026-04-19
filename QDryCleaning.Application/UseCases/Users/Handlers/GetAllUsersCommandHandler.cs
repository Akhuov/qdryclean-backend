using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Users.Quarries;

namespace QDryClean.Application.UseCases.Users.Handlers
{
    public class GetAllUsersCommandHandler : BaseHandler, IRequestHandler<GetAllUsersCommand, List<UserDto>>
    {

        public GetAllUsersCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<List<UserDto>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
        {
            var users = await _applicationDbContext.Users.ToListAsync(cancellationToken);
            var listOfUserDtos = new List<UserDto>();
            foreach (var user in users)
            {
                listOfUserDtos.Add(_mapper.Map<UserDto>(user));
            }
            return listOfUserDtos;
        }
    }
}
