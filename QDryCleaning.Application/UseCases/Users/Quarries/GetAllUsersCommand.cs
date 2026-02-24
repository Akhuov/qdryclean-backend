using MediatR;
using QDryClean.Application.Dtos;

namespace QDryClean.Application.UseCases.Users.Quarries
{
    public class GetAllUsersCommand : IRequest<List<UserDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}