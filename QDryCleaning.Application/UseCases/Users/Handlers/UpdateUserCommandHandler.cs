using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QDryClean.Application.Absreactions;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Interfaces.Services;
using QDryClean.Application.Dtos;
using QDryClean.Application.UseCases.Users.Commands;

namespace QDryClean.Application.UseCases.Users.Handlers
{
    public class UpdateUserCommandHandler : BaseHandler, IRequestHandler<UpdateUserCommand, UserDto>
    {
        public UpdateUserCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IMapper mapper) : base(applicationDbContext, currentUserService, mapper) { }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
                if (user is not null)
                {
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.LogIn = request.Login;
                    user.Password = request.Password;
                    user.UserRole = request.UserRole;
                    user.UpdatedAt = DateTime.UtcNow;
                    user.UpdatedBy = _currentUserService.UserId;

                    _applicationDbContext.Users.Update(user);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                    return _mapper.Map<UserDto>(user);
                }
                throw new BadRequestExeption($"User with ID {request.Id} not found.");
            }
            catch (BadRequestExeption)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerExeption(ex.Message);
            }
        }
    }
}
